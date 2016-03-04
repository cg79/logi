define(['app'], function(app) {
    'use strict';
    app.register.controller('MapPointsController', ['$scope', '$timeout', 'location', 'esbSharedService', 'pusherService', 'reverseGeocoder', 'utils', function($scope, $timeout, location, esbSharedService, pusherService, reverseGeocoder, utils) {
        $scope.lang = null;
        var jqxhr = $.getJSON("app/localization/en/transport_Add.html", function(resp) {
                $scope.$apply(function() {
                    $scope.lang = resp;
                });
            })
            .fail(function(xxx) {
                console.log("error loading localization");
            });



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
            //this.selectedMarker = null;
            this.selectedAddress = "";
            this.wayPointByHand = null;

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
            this.removeWayPoint = function(city) {
                //wayPoints.push(val);
                var marker = city.getMarker();
                if (marker != null) {
                    marker.setMap(null);
                }

                wayPoints = _.filter(wayPoints, function(node) {
                    return (node != city);
                });
                this.drawLine();
            }
            this.removeWayPointBymarker = function(marker) {
                //wayPoints.push(val);
                if (marker == null) {
                    return;

                }
                marker.setMap(null);
                wayPoints = _.filter(wayPoints, function(node) {
                    return (node.getMarker() != marker);
                });
                this.drawLine();
            }
            var flightPath = null;
            this.getFlightPath = function() {
                return flightPath;
            }
            this.setFlightPath = function(val) {
                flightPath = val;
                flightPath.setMap(map);
            }

            this.markerClickedEvent = function(marker) {
                // var wayPoint = _.find(wayPoints, function(node) {
                //     return (node.getMarker() == marker);
                // });
                // this.selectedMarker = wayPoint;
                // if (wayPoint != null) {
                //     this.selectedAddress = wayPoint.address.toString();
                // }
                var pos = marker.getPosition();

                var loc = new google.maps.LatLng(pos.A, pos.F);
                map.setCenter(loc);
            };

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

                    return this.c + ", " + this.s + ", " + this.z + ", " + this.t + ", " + this.st; // this.st + this.t + this.z + this.s ;

                    //return this.st + this.t + this.z + this.s + this.c;
                }
            }

            this.createCity = function(val) {
                if (val == null || val == "") {
                    console.log(" --------------------------------------     EMPTY response");
                    return;
                }
                var address = new $scope.input.route.address();
                address.parseAddress(val.address_components);
                var title = address.toString();
                // var title = "";
                // if (val[0] != undefined) {
                //     title = val[0].formatted_address;
                // }
                // if (val[1] != undefined) {
                //     title = title + " " + val[1].formatted_address;
                // }
                var afValue = val.geometry.location; //af(val[0].geometry.location.A,val[0].geometry.location.F);
                var marker = $scope.input.route.CreateMarkerFromAF(afValue, title);
                //start.setTitle(title);


                var cityObj = new $scope.input.route.city(marker, afValue, address);
                $scope.input.route.addWayPoint(cityObj);
                $scope.input.route.drawLine();
            }

            this.loadCity = function(val) {
                if (val == null || val == "") {
                    console.log(" --------------------------------------     EMPTY response");
                    return;
                }
                var address = val.address;
                var title = address.toString();
                var afValue = val.address.location; //af(val[0].geometry.location.A,val[0].geometry.location.F);
                var marker = $scope.input.route.CreateMarkerFromAF(afValue, title);
                //start.setTitle(title);


                var cityObj = new $scope.input.route.city(marker, afValue, address);
                $scope.input.route.addWayPoint(cityObj);
                $scope.input.route.drawLine();
            };

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
                    draggable: true,
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

                    $scope.input.route.markerClickedEvent(marker);

                    if (marker.selected) {

                        var content = '<h3>' + marker.title + '</h3>';

                        infoWindow.setContent(content);

                        infoWindow.open($scope.map, marker);
                    } else {
                        infoWindow.close();
                    }
                });

                google.maps.event.addListener(marker, 'dragend', function() {
                    //geocodePosition(marker.getPosition());
                    var pos = marker.getPosition();
                    var loc = {
                        latitude: pos.A,
                        longitude: pos.F,
                    };

                    $scope.input.route.removeWayPointBymarker(marker);

                    $scope.input.route.callReverseGeoCoding(loc, $scope.input.route.createCity);
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
                    latitude: this.start.geometry.location.A,
                    longitude: this.start.geometry.location.F,
                };

                this.callReverseGeoCoding(loc, this.createStartObj);


            };

            this.createStartObj = function(val) {
                if (val == null || val == "") {
                    console.log(" --------------------------------------     EMPTY response");
                    return;
                }
                var address = new $scope.input.route.address();
                address.parseAddress(val.address_components);
                var title = address.toString();

                var start = $scope.input.route.start.geometry.location;
                var marker = $scope.input.route.CreateMarkerFromAF(start, title);

                startObj = new $scope.input.route.city(marker, start, address);
                $scope.input.route.setStartObj(startObj)

                $scope.input.route.drawLine();
            };

            this.endChanged = function() {

                var endObj = this.getEndObj();
                if (isOk(endObj)) {
                    endObj.Clear();
                    this.setEndObj(null);
                }
                var loc = {
                    latitude: this.end.geometry.location.A,
                    longitude: this.end.geometry.location.F,
                };
                this.callReverseGeoCoding(loc, this.createEndObj);

            };
            this.createEndObj = function(val) {
                if (val == null || val == "") {
                    console.log(" --------------------------------------     EMPTY response");
                    return;
                }
                var address = new $scope.input.route.address();
                address.parseAddress(val.address_components);
                var title = address.toString();

                var end = $scope.input.route.end.geometry.location;
                var marker = $scope.input.route.CreateMarkerFromAF(end, title);

                endObj = new $scope.input.route.city(marker, end, address);
                $scope.input.route.setEndObj(endObj)

                $scope.input.route.drawLine();
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

                var start = this.getStartObj();
                var end = this.getEndObj();

                var request = {
                    origin: LatLng(start.address.location),
                    destination: LatLng(end.address.location),
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

                esbSharedService.mapValuesChanged();

            }

            this.CreateDBObject = function(validateStartEnd) {
                var rez = {
                    way: [],
                    dist: 0,
                    ERR: ""
                };

                var obj = this.getStartObj();
                if (obj != null) {
                    rez.way.push(obj);
                } else {
                    if (validateStartEnd) {
                        rez.ERR = "StartPoint";
                        return rez;
                    }
                }

                var waypoints = this.getWayPoints();

                for (var i = 0; i < waypoints.length; i++) {
                    rez.way.push(waypoints[i]);
                }

                obj = this.getEndObj();
                if (obj != null) {
                    rez.way.push(obj);
                } else {
                    if (validateStartEnd) {
                        rez.ERR = "EndPoint";
                        return rez;
                    }
                }


                var distV = 0;
                for (var i = 0; i < this.dist.length; i++) {
                    distV += this.dist[i].Value;
                }
                rez.dist = distV;

                try {
                    for (var i = 0; i < this.dist.length; i++) {
                        for (var k = 1; k < rez.way.length; k++) {
                            rez.way[k - 1].dist = this.dist[i].Value;
                        }
                    }
                } catch (err) {}
                return rez;
            }

        }

        $scope.$on('mapInitialized', function (e, evtMap) {
            map = evtMap;

            directionsDisplay = new google.maps.DirectionsRenderer(rendererOptions);
            directionsDisplay.setMap(map);
            directionsDisplay.setPanel(document.getElementById("directions-panel"));

            var menu = new contextMenu({
                map: map
            });

            // Add some items to the menu
            menu.addItem('Zoom In', function (map, latLng) {
                map.setZoom(map.getZoom() + 1);
                map.panTo(latLng);
            });

            menu.addItem('Add Route Location', function (map, latLng) {
                //map.panTo(latLng);


                var start = new google.maps.Marker({
                    title: "Marker: start"
                });

                var loc = {
                    latitude: latLng.A,
                    longitude: latLng.F,
                };
                $scope.input.route.callReverseGeoCoding(loc, $scope.input.route.createWayPoint);
                // var call = reverseGeocoder.geocode(loc);
                // call.then(function(val) {
                //     if (val == null || val == "") {
                //         console.log(" --------------------------------------     EMPTY response");
                //         //promise.reject();
                //         return;
                //     }


                //     var title = "";
                //     if (val[0] != undefined) {
                //         title = val[0].formatted_address;
                //     }
                //     if (val[1] != undefined) {
                //         title = title + " " + val[1].formatted_address;
                //     }
                //     start.setTitle(title);
                // });

                // $scope.input.route.addWayPoint(latLng.A, latLng.F);

                // start.setPosition(latLng);
                // start.setMap(map);


                // google.maps.event.addListener(start, 'click', function() {
                //     map.setZoom(8);
                //     map.setCenter(start.getPosition());

                // });
            });



            utils.getLocation(showPosition);
        });

        $scope.refresh = function () {
            $scope.input.route.drawLine();
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

        $scope.$watch('input.route.wayPointByHand', function() {
            if ($scope.input.route.wayPointByHand == undefined || $scope.input.route.wayPointByHand == null)
                return;
            var obj = $scope.input.route.wayPointByHand;
            $scope.input.route.createCity(obj);

            var loc = new google.maps.LatLng(obj.geometry.location.A, obj.geometry.location.F);
            map.setCenter(loc);

            $scope.input.route.selectedAddress = "";
        });


        $scope.input.route = new fRoute();

        $scope.clear = function() {
            $scope.input.route.clearPath();
        };
        $scope.cityRoutePointClick = function(e, selectedMarker) {
            e.preventDefault();
            google.maps.event.trigger(selectedMarker.getMarker(), 'click');
        }
        $scope.removeWaypoint = function(e, city) {
            e.preventDefault();

            $scope.input.route.removeWayPoint(city);
            //wayPoints = _.filter($scope.input.route.getWayPoints(), function(node) {
            //    return (node != marker);
            //});
            //$scope.input.route.drawLine();
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
                $scope.input.route.callReverseGeoCoding(loc, $scope.input.route.createCity);
            });



            utils.getLocation(showPosition);

            init();
        });

        $scope.refresh = function() {
            $scope.input.route.drawLine();
        }


        $scope.input.route = new fRoute();
        var init = function() {
            if ($scope.inputParameter != undefined) {
                $scope.$apply(function() {
                    for (var i = 0; i < $scope.inputParameter.length; i++) {
                        var obj = $scope.inputParameter[i];
                        $scope.input.route.loadCity(obj);
                    }
                });


            }
        };



    }]);
});