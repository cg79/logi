define(['app'], function(app) {
    'use strict';
    app.register.controller('CarListController', ['$scope', '$modal', '$location', 'securityService', 'pusherService', 'esbSharedService', 'utils', 'notificationService',
        function($scope, $modal, $location, securityService, pusherService, esbSharedService, utils, notificationService) {


            /*Car Begin------------------------------------------------------------------------------------------------
            ----------------------------------------------------------------------------------------------------------*/

             $scope.editMyCar = function(row) {
                var mydata = {
                    item: row
                };
                require(
                    [
                        "controllers/car/CarAddEditController"
                    ],
                    function() {
                        var modalInstance = $modal.open({
                            templateUrl: 'app/controllers/car/carAddEdit.html',
                            controller: 'CarAddEditController',
                            resolve: {
                                inputParameter: function() {
                                    return mydata;
                                }
                            }
                        });

                        modalInstance.result.then(function(selectedItem) {
                            var okPressed = $scope.SelectedOption;
                            if (okPressed == false) {
                                return;
                            }
                            $scope.loadCarLists();
                        }, function() {
                            //$log.info('Modal dismissed at: ' + new Date());
                        });
                    });

                return;
                $scope.CarList.ID = 0;
                $scope.CarList.Name = "";
                $scope.CarList.ApplicationID = 0;
                $scope.SecurityViewPath = "app/views/security/CarList.html?id=1";
            };
            
            $scope.editCarList = function(CarList) {
                var mydata = {
                    CarList: CarList
                };
                require(
                    [
                        "controllers/security/AddEditCarListController"
                    ],
                    function() {
                        var modalInstance = $modal.open({
                            templateUrl: 'app/views/security/editCarList.html?id=1',
                            controller: 'AddEditCarListController',
                            resolve: {
                                inputParameter: function() {
                                    return mydata;
                                }
                            }
                        });
                        modalInstance.result.then(function(selectedItem) {
                            var okPressed = $scope.SelectedOption;
                            if (okPressed == false) {
                                return;
                            }
                            $scope.loadCarLists();
                        }, function() {
                            //$log.info('Modal dismissed at: ' + new Date());
                        });
                    });
            };

            $scope.execAddUpdateCarList = function() {
                if (this.CarListForm.$invalid) {
                    //alert("invalid");
                    return;
                }

                $scope.CarList.ServerError = "";
                //var jsonData = $("#createUserForm").serialize();
                var methodInfo = {
                    AssemblyName: "ESB.Security.DAL.dll",
                    NameSpace: "ESB.Security.DAL.SecurityCarLists",
                    MethodName: "SAddCarList"
                };
                var esbRequest = {
                    restUrl: "api/security/AddCarList",
                    correlationId: "",
                    messageType: 1 | 4,
                    methodInfo: methodInfo,
                    jsonRequest: JSON.stringify($scope.CarList),
                };
                pusherService.PushESBMessage(esbRequest, $scope.OnHandleCarListResponse, $scope);
            }

            $scope.deleteCarList = function(CarList) {

                var mydata = {
                    message: "Are you sure you want to delete the <b>" + CarList.Name + "</b> CarList ?"
                };
                require(
                    [
                        "controllers/ConfirmationController"
                    ],
                    function() {
                        var modalInstance = $modal.open({
                            templateUrl: 'app/views/confirmation.html?id=1',
                            controller: 'ConfirmationController',
                            resolve: {
                                inputParameter: function() {
                                    return mydata;
                                }
                            }
                        });

                        modalInstance.result.then(function(selectedItem) {
                            var okPressed = $scope.SelectedOption;
                            if (okPressed == false) {
                                return;
                            }
                            var methodInfo = {
                                AssemblyName: "ESB.Security.DAL.dll",
                                NameSpace: "ESB.Security.DAL.SecurityCarLists",
                                MethodName: "SDeleteCarList"
                            };
                            var esbRequest = {
                                restUrl: "api/security/RemoveCarList",
                                correlationId: "",
                                messageType: 1 | 4,
                                methodInfo: methodInfo,
                                jsonRequest: JSON.stringify(CarList),
                            };
                            pusherService.PushESBMessage(esbRequest, $scope.OnHandleDeleteCarListResponse, $scope);
                        }, function() {
                            //$log.info('Modal dismissed at: ' + new Date());
                        });
                    });
            };

            $scope.OnHandleDeleteCarListResponse = function(obj, error) {
                if (obj.ResponseCode != null && obj.ResponseCode != '') {
                    notificationService.ShowLocalizationMessage("CarList", obj.LocalizationCode, 1);
                    return;
                }
                notificationService.ShowLocalizationMessage("CarList", obj.LocalizationCode);
                $scope.loadCarLists();
            };

            $scope.OnHandleCarListResponse = function() {
                $scope.loadCarLists();
                $scope.SecurityViewPath = "";
            };

            $scope.searchTextCarList = "";
            $scope.triggerCarListFilter = function() {
                if ($scope.CarListsPager.Pager.currentPage > 1) {
                    $scope.CarListsPager.Pager.currentPage = 1;
                    return;
                }
                $scope.loadCarLists();
            }

            $scope.CarLists = [];
            $scope.CarListsPager = new utils.PagerInstance();
            $scope.CarListGridColumns =
                [
                {
                    Name: 'Marca',
                    FriendlyName: 'Marca',
                    SortOrder: 0,
                    Filter: null,
                    Selected: false
                },
                {
                    Name: 'Model',
                    FriendlyName: 'Model',
                    SortOrder: 0,
                    Filter: null,
                    Selected: false
                },
                {
                    Name: 'Tip_M_ID',
                    FriendlyName: 'Tip Masina',
                    SortOrder: 0,
                    Filter: null,
                    Selected: false
                }
                ];

            $scope.sortChangedCarList = function(column, asc) {
                //alert("sort changed " + column + asc);
                var smartColumn = _.find($scope.CarListGridColumns, function(o) {
                    return o.Name === column;
                });
                if (smartColumn == null)
                    return;

                smartColumn.SortOrder = asc;

                $scope.loadCarLists();
            };
            $scope.$watch('CarListsPager.Pager.currentPage', function(newValue, oldValue) {
                // Do anything you like here
                if (newValue == undefined || newValue == 0 || newValue === oldValue) {
                    return;
                }

                $scope.loadCarLists();
            });

            $scope.$watch('CarListsPager.Pager.pageSize', function(newValue, oldValue) {
                // Do anything you like here
                if (newValue == undefined || newValue == 0 || newValue === oldValue) {
                    return;
                }
                $scope.loadCarLists();
            });
            $scope.loadCarLists = function() {
               

                var pager = {
                    PageIndex: $scope.CarListsPager.Pager.currentPage,
                    RowsPerPage: $scope.CarListsPager.Pager.pageSize,
                    SortOrder: [],
                    FilterCriteria: []
                };

                for (var i = 0; i < $scope.CarListGridColumns.length; i++) {
                    var col = $scope.CarListGridColumns[i];
                    if (col.SortOrder == 0)
                        continue;

                    var sortCriteria = new
                    $scope.CarListsPager.SortCriteria(col.Name, col.SortOrder == 1 ? true : false)
                    pager.SortOrder.push(sortCriteria);
                }
                if ($scope.searchTextCarList != "") {
                    var selectedColumns = _.findWhere($scope.CarListGridColumns, {
                        Selected: true
                    });
                    for (var i = 0; i < $scope.CarListGridColumns.length; i++) {
                        var col = $scope.CarListGridColumns[i];
                        if (col.Selected == false && selectedColumns != undefined)
                            continue;
                        var filterCrit = {
                            FieldName: col.Name,
                            Operator: 8,
                            Value: $scope.searchTextCarList
                        };

                        pager.FilterCriteria.push(filterCrit);
                    }
                }

                    var filterCrit = {
                            FieldName: "UserGuid",
                            Operator: 8,
                            Value: esbSharedService.user.Guid
                        };

                        pager.FilterCriteria.push(filterCrit);
                
 
               var mmm = {
                    Library: "UsersImplementation",
                    Namespace: "UsersImplementation.Repositories.UserCarRepository",
                    Method: "GetPage",
                    JSON: ""
                };

                mmm.JSON = JSON.stringify(pager);
                pusherService.PushESBMessage(mmm, $scope.DBCarListsReceived, $scope);
            }

            $scope.DBCarListsReceived = function(obj, error) {
                if (error != null) {
                    alert(JSON.stringify(error));
                    return;
                }

                var items = obj.Items;
                for (var i = 0; i < items.length; i++) {
                    Object.defineProperty(items[i], 'Selected', {
                        value: false,
                        writable: true,
                        enumerable: true,
                        configurable: true
                    });

                    items[i].selection = function() {
                        if (this.Selected) {
                            return "sel";
                        }
                        return "not_sel";
                    }

                }
                $scope.CarLists = items;
                $scope.CarListsPager.SetTotalItems(obj.TotalRecords);
            };

            $scope.SecurityViewPath = "";

            $scope.CarListSelected = function(selectedCarList) {


                $scope.SecurityViewPath = "app/views/security/CarList.html?id=1";
                //$scope.$apply(function () {
                $scope.CarList = selectedCarList;
                //});
            };

           
            /*CarLists End------------------------------------------------------------------------------------------------
            ----------------------------------------------------------------------------------------------------------*/



            //User End----

            $scope.loadCarLists();
            $scope.$on('$destroy', function() {
                console.log('carlist  is no longer necessary');
            })



        }
    ]);

});