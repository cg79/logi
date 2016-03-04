define(['app'], function(app) {
    'use strict';
    app.register.controller('TransportAddController', ['$scope', '$timeout', 'location', 'esbSharedService', 'pusherService', 'reverseGeocoder', 'utils', function($scope, $timeout, location, esbSharedService, pusherService, reverseGeocoder, utils) {
        $scope.lang = null;
        var jqxhr = $.getJSON("app/localization/en/transport_Add.html", function(resp) {
                $scope.$apply(function() {
                    $scope.lang = resp;
                });
            })
            .fail(function(xxx) {
                console.log("error loading localization");
            });

        $scope.openDate = function($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.data.opened = true;
        };

        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };

        $scope.data = {
            opened: false,
            val: null,
            myCars: []

        };

        var inputData = function() {
            this.CarGuid = null;
            this.startDate = new Date();
            this.startTime = new Date();
            this.hstep = 1;
            this.mstep = 15;
            this.useRecurrence = false;
            this.description = "";
            this.route = null;
            this.recCriteria = null;
            this.ERR = "sdf";
            this.CreateDBObject = function() {
                var rez = {
                    UserGuid: esbSharedService.user.Guid,
                    CarGuid: this.CarGuid,
                    StartDate: null,
                    Rec: null,
                    Desc: this.description,
                    Route: {
                        way: [],
                        dist: 0
                    },
                    ERR: null,
                    Valid: function() {
                        if (this.CarGuid == null)
                            return "CarGuid";

                        return null;
                    }
                };
                var d1 = utils.Date(this.startDate);
                var d2 = (this.startTime != null) ? utils.Date(this.startTime) : utils.Date(new Date());

                var dataPornire = new Date(d1.year, d1.month, d1.day, d2.hour, d2.minute, d2.seconds);
                rez.StartDate = utils.Date(dataPornire);

                rez.ERR = null;

                if (this.useRecurrence == true) {
                    rez.Rec = this.recCriteria;
                }

                var obj = this.route.getStartObj();
                if (obj == null) {
                    rez.ERR = "StartPoint";
                    return rez;
                }

                rez.Route.way.push(obj);
                var waypoints = this.route.getWayPoints();

                for (var i = 0; i < waypoints.length; i++) {
                    rez.Route.way.push(waypoints[i]);
                }
                obj = this.route.getEndObj();
                if (obj == null) {
                    rez.ERR = "EndPoint";
                    return rez;
                }
                rez.Route.way.push(obj);

                var distV = 0;
                for (var i = 0; i < this.route.dist.length; i++) {
                    distV += this.route.dist[i].Value;
                }
                rez.Route.dist = distV;

                try {
                    for (var i = 0; i < this.route.dist.length; i++) {
                        for (var k = 1; k < rez.Route.way.length; k++) {
                            rez.Route.way[k - 1].dist = this.route.dist[i].Value;
                        }
                    }
                } catch (err) {}

                // var car = _.find($scope.data.myCars, function(o) {
                //     return o.CarGuid == rez.CarGuid;
                // });
                // rez.Car = car;

                return rez;
            }
        }


        $scope.input = new inputData();



        //$scope.$watch('input.route.start', function() {
        //    if ($scope.input.route == undefined || $scope.input.route.start == null)
        //        return;

        //    $scope.input.route.startChanged();

        //});


        //$scope.$watch('input.route.end', function() {

        //    if ($scope.input.route == undefined || $scope.input.route.end == null)
        //        return;

        //    $scope.input.route.endChanged();

        //});


        $scope.clear = function() {
            $scope.input.route.clearPath();
        };
        $scope.cityRoutePointClick = function(e, selectedMarker) {
            e.preventDefault();
            google.maps.event.trigger(selectedMarker.marker, 'click');
        }
        $scope.removeWaypoint = function(e, selectedMarker) {
            e.preventDefault();
            selectedMarker.marker.setMap(null);
            $scope.input.route.wayPoints = _.filter($scope.input.route.wayPoints, function(node) {
                return (node != selectedMarker);
            });
            $scope.input.route.drawLine();
        }


        function showPosition(position) {
            //map.setCenter(position.coords, 14);
            var loc = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);


            map.setCenter(loc);

        }
       

        $scope.init = function() {
            //$scope.$apply(function() {

            //$scope.input.route.start = new af(46.766667, 23.58333300000004);
            //$scope.input.route.end = new af(47.133333, 24.5);
            //});

            require(
                [
                    "controllers/map/MapPointsController"
                ],
                function() {
                    $scope.$apply(function() {
                        $scope.MapPath = 'app/controllers/map/mapPoints.html';
                    });
                });

            var mmm = {
                Library: "UsersImplementation",
                Namespace: "UsersImplementation.Repositories.UserCarRepository",
                Method: "GetMyCars",
                JSON: ""
            };
            var msg = {
                Guid: esbSharedService.user.Guid,
            };
            mmm.JSON = JSON.stringify(msg);

            pusherService.PushESBMessage(mmm, $scope.OnGetMyCars, $scope);
        }
        $scope.OnGetMyCars = function(obj) {
            $scope.data.myCars = obj;
        }

        $scope.SchedPath = "";
        $scope.ShowHideRecurrence = function() {
                if ($scope.input.useRecurrence == false)
                    return;

                require(
                    [
                        "controllers/scheduler/schedulerController"
                    ],
                    function() {
                        $scope.$apply(function() {
                            $scope.SchedPath = "app/controllers/scheduler/schedulerui.html?id=1";
                        });
                    });
            }
            //$scope.aaaa = null;
        $scope.saveOfertaTransport = function() {
            $scope.input.ERR = "";
            var mmm = {
                Library: "UsersImplementation",
                Namespace: "UsersImplementation.Repositories.UserCarRepository",
                Method: "SaveOfertaTransport",
                JSON: ""
            };
            var msg = $scope.input.CreateDBObject();
            //$scope.aaaa = msg;
            //validate request
            if (msg.ERR != null) {
                $scope.input.ERR = $scope.lang[msg.ERR];
                return;
            }
            var validationMsg = msg.Valid();
            if (validationMsg != null) {
                $scope.input.ERR = $scope.lang[validationMsg];
                return;
            }

            mmm.JSON = JSON.stringify(msg);

            pusherService.PushESBMessage(mmm, $scope.ofertaSaved, $scope);
        }
        $scope.ofertaSaved = function(obj) {

        }

        $scope.MapPath = "";


        $scope.init();

    }]);
});