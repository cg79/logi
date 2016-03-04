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


        var map;
        var rendererOptions = {
            draggable: true
        };
        var directionsDisplay;
        var directionsService = new google.maps.DirectionsService();
        var infoWindow = new google.maps.InfoWindow();


        var txtVal = function(txt, val) {
            this.Text = txt;
            this.Value = val;
        }
        var LatLng = function(location) {
            var res = new google.maps.LatLng(location.A, location.F);
            return res;
        }
        var af = function(a, f) {
            this.A = a;
            this.F = f;
        }

        var isOk = function(obj) {
            if (obj == undefined || obj == null)
                return false;
            return true;
        }


        var fRoute = function() {
            this.start = null;
            this.end = null;

            var startObj = null;
            this.setStartObj = function(it) {
                startObj = it;
            }
            this.getStartObj = function() {
                return startObj;
            }

            var endObj = null;
            this.setEndObj = function(it) {
                endObj = it;
            }
            this.getEndObj = function() {
                return endObj;
            }



            var wayPoints = [];
            this.setWayPoints = function(val) {
                wayPoints = val;
            }
            this.getWayPoints = function() {
                return wayPoints;
            }
            this.addWayPoint = function(val) {
                wayPoints.push(val);
            }

            var flightPath = null;
            this.getFlightPath = function() {
                return flightPath;
            }
            this.setFlightPath = function(val) {
                flightPath = val;
                flightPath.setMap(map);
            }

            this.dist = [];
            this.dur = [];

            this.city = function(marker, af, address) {
                var marker = marker;
                this.getMarker = function() {
                        return marker;
                    }
                    //this.location = af;
                address.location = af;
                this.address = address;

                this.dist = 0;
                this.Clear = function() {
                    marker.setMap(null);
                    marker = null;
                }
            };

            this.address = function() {
                this.c = "";
                this.s = "";
                this.z = "";
                this.t = "";
                this.st = "";
                this.location = null;
                this.parseAddress = function(cmp) {
                    for (var i = 0; i < cmp.length; i++) {
                        switch (i) {
                            case 0:
                                this.st = cmp[i].short_name;
                                break;
                            case 1:
                                this.t = cmp[i].short_name;
                                break;
                            case 2:
                                this.z = cmp[i].short_name;
                                break;
                            case 3:
                                this.s = cmp[i].short_name;
                                break;
                            case 4:
                                this.c = cmp[i].short_name;
                                break;
                        }
                    }
                }
                this.toString = function() {
                    return this.st + this.t + this.z + this.s + this.c;
                }
            }

            this.createCity = function(val) {
                if (val == null || val == "") {
                    console.log(" --------------------------------------     EMPTY response");
                    return;
                }
                var address = new $scope.input.route.address();
                address.parseAddress(val[0].address_components);
                var title = address.toString();
                // var title = "";
                // if (val[0] != undefined) {
                //     title = val[0].formatted_address;
                // }
                // if (val[1] != undefined) {
                //     title = title + " " + val[1].formatted_address;
                // }
                var afValue = val[0].geometry.location; //af(val[0].geometry.location.A,val[0].geometry.location.F);
                var marker = $scope.input.route.CreateMarkerFromAF(afValue, title);
                //start.setTitle(title);


                var cityObj = new $scope.input.route.city(marker, afValue, address);
                $scope.input.route.addWayPoint(cityObj);
            }
            this.createWayPoint = function(val) {
                if (val == null || val == "") {
                    console.log(" --------------------------------------     EMPTY response");
                    return;
                }
                var address = new $scope.input.route.address();
                address.parseAddress(val[0].address_components);
                var title = address.toString();
                // var title = "";
                // if (val[0] != undefined) {
                //     title = val[0].formatted_address;
                // }
                // if (val[1] != undefined) {
                //     title = title + " " + val[1].formatted_address;
                // }
                var afValue = val[0].geometry.location; //af(val[0].geometry.location.A,val[0].geometry.location.F);
                var marker = $scope.input.route.CreateMarkerFromAF(afValue, title);
                //start.setTitle(title);


                var cityObj = new $scope.input.route.city(marker, afValue, address);
                $scope.input.route.addWayPoint(cityObj);

                $scope.input.route.drawLine();
            }
            this.tryAddCity = function(location) {
                this.callReverseGeoCoding(location, this.createCity);
                //
            };

            this.callReverseGeoCoding = function(loc, callback) {
                var call = reverseGeocoder.geocode(loc);
                call.then(function(val) {
                    $timeout(function() {
                        callback(val);
                    });
                });
            };

            this.CreateMarkerFromAF = function(af, title) {
                var marker = new google.maps.Marker({
                    title: title
                });

                var loc = new LatLng(af);

                var marker = new google.maps.Marker({
                    map: map,
                    position: loc,
                    title: title,
                    selected: false
                });
                marker.content = '<div class="infoWindowContent">' + title + '</div>';

                google.maps.event.addListener(marker, 'click', function(useApply) {
                    if (isOk(useApply) && useApply) {
                        $scope.$apply(function() {
                            marker.selected = !marker.selected;
                        });
                    } else {
                        marker.selected = !marker.selected;
                    }

                    if (marker.selected) {
                        infoWindow.setContent('<h3>' + marker.title + '</h3>' + marker.content);
                        infoWindow.open($scope.map, marker);
                    } else {
                        infoWindow.close();
                    }
                });

                return marker;

            }


            this.startChanged = function() {
                var startObj = this.getStartObj();
                if (isOk(startObj)) {
                    startObj.Clear();
                    this.setStartObj(null);
                }
                var loc = {
                    latitude: this.start.A,
                    longitude: this.start.F,
                };

                this.callReverseGeoCoding(loc, this.createStartObj);


            };

            this.createStartObj = function(val) {
                if (val == null || val == "") {
                    console.log(" --------------------------------------     EMPTY response");
                    return;
                }
                var address = new $scope.input.route.address();
                address.parseAddress(val[0].address_components);
                var title = address.toString();

                var start = $scope.input.route.start;
                var marker = $scope.input.route.CreateMarkerFromAF(start, title);

                startObj = new $scope.input.route.city(marker, start, address);
                $scope.input.route.setStartObj(startObj)

                $scope.input.route.drawLine();

                $scope.searchRutaTransport();
            };

            this.endChanged = function() {

                var endObj = this.getEndObj();
                if (isOk(endObj)) {
                    endObj.Clear();
                    this.setEndObj(null);
                }
                var loc = {
                    latitude: this.end.A,
                    longitude: this.end.F,
                };
                this.callReverseGeoCoding(loc, this.createEndObj);


            };
            this.createEndObj = function(val) {
                if (val == null || val == "") {
                    console.log(" --------------------------------------     EMPTY response");
                    return;
                }
                var address = new $scope.input.route.address();
                address.parseAddress(val[0].address_components);
                var title = address.toString();

                var end = $scope.input.route.end;
                var marker = $scope.input.route.CreateMarkerFromAF(end, title);

                endObj = new $scope.input.route.city(marker, end, address);
                $scope.input.route.setEndObj(endObj)

                $scope.input.route.drawLine();
                $scope.searchRutaTransport();
            };


            this.clearPath = function() {
                if (flightPath == null)
                    return;

                flightPath.setMap(null);
                flightPath = null;
            }

            this.drawLine = function() {
                if (this.start == null || this.start == undefined)
                    return;

                if (this.end == null || this.end == undefined)
                    return;


                this.clearPath();

                var ways = [];
                var waypoints = this.getWayPoints();
                for (var i = 0; i < waypoints.length; i++) {
                    ways.push({
                        location: waypoints[i].address.location,
                        stopover: true
                    })
                }
                var request = {
                    origin: LatLng(this.start),
                    destination: LatLng(this.end),
                    waypoints: ways,
                    travelMode: google.maps.TravelMode.DRIVING
                };
                directionsService.route(request, function(response, status) {
                    if (status == google.maps.DirectionsStatus.OK) {
                        //directionsDisplay.setDirections(response);
                        $timeout(function() {
                            var legs = response.routes[0].legs;
                            $scope.input.route.dist = [];
                            $scope.input.route.dur = [];
                            for (var i = 0; i < legs.length; i++) {
                                $scope.input.route.dist.push(new txtVal(legs[i].distance.text, legs[i].distance.value));
                                $scope.input.route.dur.push(new txtVal(legs[i].duration.text, legs[i].duration.value));
                            }

                            var op = response.routes[0].overview_path;

                            var fPath = new google.maps.Polyline({
                                path: op,
                                geodesic: true,
                                strokeColor: '#FF0000',
                                strokeOpacity: 1.0,
                                strokeWeight: 5
                            });
                            $scope.input.route.setFlightPath(fPath);


                            google.maps.event.addListener(fPath, 'click', function(clickEvent, a, b) {
                                //did you want to do something here??
                                var loc = {
                                    latitude: clickEvent.latLng.A,
                                    longitude: clickEvent.latLng.F,
                                };
                                $scope.input.route.tryAddCity(loc);
                            });
                        });
                    }
                });

            }
        }



        $scope.$watch('input.route.start', function() {
            if ($scope.input.route == undefined || $scope.input.route.start == null)
                return;

            $scope.input.route.startChanged();

        });


        $scope.$watch('input.route.end', function() {

            if ($scope.input.route == undefined || $scope.input.route.end == null)
                return;

            $scope.input.route.endChanged();

        });

        $scope.input.route = new fRoute();

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

        $scope.$on('mapInitialized', function(e, evtMap) {
            map = evtMap;

            directionsDisplay = new google.maps.DirectionsRenderer(rendererOptions);
            directionsDisplay.setMap(map);
            directionsDisplay.setPanel(document.getElementById("directions-panel"));

            var menu = new contextMenu({
                map: map
            });

            // Add some items to the menu
            menu.addItem('Zoom In', function(map, latLng) {
                map.setZoom(map.getZoom() + 1);
                map.panTo(latLng);
            });

            menu.addItem('Add Route Location', function(map, latLng) {
                //map.panTo(latLng);


                var start = new google.maps.Marker({
                    title: "Marker: start"
                });

                var loc = {
                    latitude: latLng.A,
                    longitude: latLng.F,
                };
                $scope.input.route.callReverseGeoCoding(loc, $scope.input.route.createWayPoint);

            });



            utils.getLocation(showPosition);
        });

        $scope.refresh = function() {
            $scope.input.route.drawLine();
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