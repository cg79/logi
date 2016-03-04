define(['app'], function(app) {
    'use strict';
    app.register.controller('CompanyLocationsController', ['$scope', '$modal', '$location', '$timeout', '$modalInstance', 'securityService', 'pusherService', 'esbSharedService', 'utils', 'inputParameter',
        function($scope, $modal, $location, $timeout, $modalInstance, securityService, pusherService, esbSharedService, utils, inputParameter) {

            var cmpInfoObj = function() {
                this.Point = null;
            }

            $scope.input = new cmpInfoObj();
            $scope.inputParameter= inputParameter;


            //fill the userd data
            $scope.ui = null;
            var jqxhr = $.getJSON("app/localization/en/user.html", function(resp) {
                    $scope.$apply(function() {
                        $scope.ui = resp;
                    });

                })
                .fail(function(xxx) {
                    console.log("error loading car ui");
                });


            $scope.MapPath = "";
            $scope.init = function() {
                require(
                    [
                        "controllers/map/MapPointsController"
                    ],
                    function() {
                        $scope.$apply(function() {
                            $scope.MapPath = 'app/controllers/map/mapPoints.html';
                        });
                    });
            }

            $scope.closeRouteDlgWithYes = function() {
                var rez = $scope.input.route.CreateDBObject();
                $modalInstance.close(rez);
            }

            $scope.init();

        }
    ]);

});