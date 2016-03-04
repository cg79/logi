define(['app'], function(app) {
    'use strict';
    app.register.controller('TransportSearchController', ['$scope', '$timeout', 'location', 'esbSharedService', 'pusherService', 'reverseGeocoder', 'utils', function($scope, $timeout, location, esbSharedService, pusherService, reverseGeocoder, utils) {
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
                    StartDate: null,
                    Rec: null,
                    Desc: this.description,
                    Route: {
                        way: [],
                        dist: 0
                    },
                    ERR: null
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

                return rez;
            }
        }


        $scope.input = new inputData();



      

        $scope.$on('onMapValuesChanged', function () {
            if ($scope.input.route == undefined || $scope.input.route.start == null)
                return;
            if ($scope.input.route == undefined || $scope.input.route.end == null)
                return;

            $scope.searchRutaTransport();
        });

     
        $scope.init = function() {
            //$scope.$apply(function() {

            //$scope.input.route.start = new af(46.766667, 23.58333300000004);
            //$scope.input.route.end = new af(47.133333, 24.5);
            //});

            require(
               [
                   "controllers/map/MapPointsController"
               ],
               function () {
                   $scope.$apply(function () {
                       $scope.MapPath = 'app/controllers/map/mapPoints.html';
                   });
               });

        }


        $scope.searchRutaTransport = function() {
            $scope.input.ERR = "";
            var mmm = {
                Library: "UsersImplementation",
                Namespace: "UsersImplementation.Repositories.TransportOfferRepository",
                Method: "SearchOfertaTransport",
                JSON: ""
            };
            var msg = $scope.input.CreateDBObject();
            //$scope.aaaa = msg;
            //validate request
            if (msg.ERR != null) {
                $scope.input.ERR = $scope.lang[msg.ERR];
                return;
            }


            mmm.JSON = JSON.stringify(msg);

            pusherService.PushESBMessage(mmm, $scope.searchRutaTransportReceived, $scope);
        }


        var path = function(obj)
        {
            // var rez = {
            //     FirstName:"",
            //     UserImg:"",
            //     TipVehicle:0,
            //     Marca:"",
            //     IsRepetitive:false,
            //     IsTwoWay:false,
            //     StartDate:null,
            //     Amount:0,
            //     Company:null
            // };
            var rez = [];
            var p1=function()
            {
                this.r =null;
                this.c=null;
                this.u=null;

                this.inf=null;
                this.init = function()
                {
                    this.inf = {
                        VImg:"",
                        Marca:"",
                        NrTotalLocuri:4,
                        Tip:"",
                        UserName:"",
                        UserImg:"",
                        Price:0,
                        IsRepetitive  : (this.r.Rec != null),
                        StartDate:this.r.StartDate.isoDate,
                        IsTwoWay: false,
                        Dist:this.r.Route.dist
                    }
                    if(this.c != undefined)
                    {
                        if(this.c.ImgUrl ==undefined || this.c.ImgUrl == "")
                        {
                            this.inf.VImg = "images/no_car.png";
                        }else{
                            this.inf.VImg= "images/"+ this.c.ImgUrl;
                        }
                        this.inf.Marca =this.c.Marca + '-' + this.c.Model;
                        this.inf.NrTotalLocuri= this.c.NrLocuri;
                        this.inf.Tip =  this.c.Tip_M_ID;
                    }
                    if(this.u != undefined)
                    {
                        this.inf.UserName = this.u.FirstName!= undefined? this.u.FirstName:"";
                        if(this.u.ImgUrl == "")
                        {
                            this.inf.UserImg = "images/no_avatar.png";
                        }else{
                            this.inf.UserImg = "images/Avatar/" +this.u.ImgUrl ;
                        }
                       
                    }


                }
            };

            for(var i=0;i<obj.routes.length;i++)
            {
               var poss = new p1();
               poss.r = obj.routes[i];
                poss.c = _.find(obj.cars, function(o) {
                    return o.CarGuid == poss.r.CarGuid;
                });
                poss.u = _.find(obj.users, function(o) {
                    return o.Guid == poss.r.UserGuid;
                });
                poss.init();    
                rez.push(poss);
            }

            return rez;
            
        };

        $scope.rez = null;
        $scope.searchRutaTransportReceived = function(obj) {
            $scope.RList = path(obj);
        }

        $scope.init();

        $scope.RList = [];
        $scope.RPager = new utils.PagerInstance();
        $scope.RColumns =
            [{
                Name: 'CarGuid',
                FriendlyName: 'Marca',
                SortOrder: 0,
                Filter: null,
                Selected: false
            }, {
                Name: 'Model',
                FriendlyName: 'Model',
                SortOrder: 0,
                Filter: null,
                Selected: false
            }, {
                Name: 'Tip_M_ID',
                FriendlyName: 'Tip Masina',
                SortOrder: 0,
                Filter: null,
                Selected: false
            }];

            $scope.$watch('RPager.Pager.currentPage', function(newValue, oldValue) {
                // Do anything you like here
                if (newValue == undefined || newValue == 0 || newValue === oldValue) {
                    return;
                }

                $scope.searchRutaTransport();
            });



    }]);
});