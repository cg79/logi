debugger;
define(['app'], function (app) {
    'use strict';
    app.register.controller('UsersController',
        ['$scope', '$modal', '$location', 'securityService', 'pusherService', 'esbSharedService', 'utils', 'notificationService',
            function ($scope, $modal, $location, securityService, pusherService, esbSharedService, utils, notificationService) {

        $scope.ControllerName = "UsersController";
        $scope.user =
        {
            Login: "",
            Password: "",
            RememberMe: false,
            Email: "",
            ServerError: ""
        };
        $scope.role =
             {
                 ID: 0,
                 Name: "",
                 ApplicationID: 0,
                 ServerError: ""
             };
        var localizationMessages = esbSharedService.localization;


      
        $scope.showPermissionsPopup = function (id, text, event) {
            if (event != undefined) {
                event.preventDefault();
                event.stopPropagation();
            }
            var mydata = {
                id: id,
                featureName: text,
            };
            require(
                       [
                           "controllers/framework/securityPermissionsController"
                       ],
                        function () {
                            var modalInstance = $modal.open({
                                templateUrl: 'app/views/framework/security/permissionRoles.html?id=1',
                                controller: 'SecurityPermissionsController',
                                resolve: {
                                    inputParameter: function () {
                                        return mydata;
                                    }
                                }
                            });
                        });
        };

        $scope.$on('onPermissionsChanged', function () {
            //$scope.$apply(function () {
            $scope.permissions = securityService.permissions;
            $scope.loadSecurityUsers();
        });

        $scope.executeFunction = function () {
            if(securityService.securityToken == "empty")
              return;

            $scope.loadSecurityUsers();
            if (esbSharedService.functionName == null || esbSharedService.functionName == "")
                return;
            var parts = esbSharedService.functionName.split(".");
            if (parts.length != 2) {
                console.log("controller and function not specified");
                return;
            }

            if (parts[0] != $scope.ControllerName) {
                return;
            }

            $scope[parts[1]]();
        }



     
      
        /*User Begin------------------------------------------------------------------------------------------------
        ----------------------------------------------------------------------------------------------------------*/
        $scope.newUser = function () {
            var user =
            {
                ID: 0,
                Login: "",
                FirstName: "",
                LastName: "",
                Password: "",
                Email: ""
            };
            return user;
        };
        $scope.users = [];
        $scope.user = null;
        $scope.UsersPager = new utils.PagerInstance();

        $scope.$watch('UsersPager.Pager.currentPage', function (newValue, oldValue) {
            // Do anything you like here
            if (newValue == undefined || newValue == 0 || newValue === oldValue) {
                return;
            }

            $scope.loadSecurityUsers();
        });
        $scope.$watch('UsersPager.Pager.pageSize', function (newValue, oldValue) {
            // Do anything you like here
            if (newValue == undefined || newValue == 0 || newValue === oldValue) {
                return;
            }
            $scope.loadSecurityUsers();
        });

        $scope.sortChangedUser = function (column, asc) {
            //alert("sort changed " + column + asc);
            var smartColumn = _.find($scope.userGridColumns, function (o) { return o.Name === column; });
            if (smartColumn == null)
                return;

            smartColumn.SortOrder = asc;

            $scope.loadSecurityUsers();
        };
        $scope.userGridColumns =
        [
             { "Name": "Login", "FriendlyName": "Login Name", SortOrder: 0, Filter: null, Selected: false },
        { "Name": "FirstName", "FriendlyName": "First Name", SortOrder: 0, Filter: null, Selected: false },
        { "Name": "LastName", "FriendlyName": "Last Name", SortOrder: 0, Filter: null, Selected: false },
            { "Name": "Email", "FriendlyName": "Email", SortOrder: 0, Filter: null, Selected: false }
        ];
        $scope.searchTextUser = "";
        $scope.triggerUserFilter = function () {
            if ($scope.UsersPager.Pager.currentPage > 1) {
                $scope.UsersPager.Pager.currentPage = 1;
                return;
            }
            $scope.loadSecurityUsers();
        }
        $scope.loadSecurityUsers = function () {
            var methodInfo =
                {
                    AssemblyName: "ESB.Security.DAL.dll",
                    NameSpace: "ESB.Security.DAL.SecurityUsers",
                    MethodName: "SGetPage"

                };
            var pager =
                {
                    PageIndex: $scope.UsersPager.Pager.currentPage,
                    RowsPerPage: $scope.UsersPager.Pager.pageSize,
                    SortOrder: [],
                    FilterCriteria: []
                };

            for (var i = 0; i < $scope.userGridColumns.length; i++) {
                var col = $scope.userGridColumns[i];
                if (col.SortOrder == 0)
                    continue;

                var sortCriteria = new
                    $scope.UsersPager.SortCriteria(col.Name, col.SortOrder == 1 ? true : false)
                pager.SortOrder.push(sortCriteria);
            }
            if ($scope.searchTextUser != "") {
                var selectedColumns = _.findWhere($scope.userGridColumns, { Selected: true });

                for (var i = 0; i < $scope.userGridColumns.length; i++) {
                    var col = $scope.userGridColumns[i];
                    if (col.Selected == false && selectedColumns != undefined)
                        continue;
                    var filterCrit = {
                        FieldName: col.Name,
                        Operator: 8,
                        Value: $scope.searchTextUser
                    };

                    pager.FilterCriteria.push(filterCrit);
                }
            }

            var correlationId = "";
            var correlationId = "";
            var esbRequest =
                        {
                            restUrl: "api/security/LoadSecurityUsers",
                            correlationId: "",
                            messageType: 1 | 4,
                            methodInfo: methodInfo,

                        };
            esbRequest.jsonRequest = JSON.stringify(pager);
            pusherService.PushESBMessage(esbRequest, $scope.OnHandleSecurityUsers, $scope);
        };

        $scope.OnHandleSecurityUsers = function (obj, error) {
            if (error != null) {
                alert(JSON.stringify(error));
                return;
            }
            $scope.users = [];
            $scope.UsersPager.SetTotalItems(obj.TotalRecords);

            $scope.users = obj.Items;
            for (var i = 0; i < $scope.users.length; i++) {
                Object.defineProperty($scope.users[i], 'Selected', {
                    value: false,
                    writable: true,
                    enumerable: true,
                    configurable: true
                })
            }
        };

        $scope.userSelected = function (selectedUser) {

            // open now from popup
            //$scope.SecurityViewPath = "app/views/framework/security/user.html?id=1";
            //$scope.$apply(function () {
            _.each($scope.users, function (it) {
                if (it.css != undefined) {
                    it.css = "";
                }
            });

            if (selectedUser.css == undefined) {
                Object.defineProperty(selectedUser, 'css', {
                    value: "st-selected",
                    writable: true,
                    enumerable: true,
                    configurable: true
                });
            } else {
                selectedUser.css = "st-selected";
            }
            //});
        };
        //note 2 -- Open user forms in popup----------------------------------------------------------------------
        $scope.createUser = function (user) {
            //$scope.user = $scope.newUser();
            var mydata = {
                user: user
            };

            require(
                       [
                           "controllers/framework/security/AddEditUserController"
                       ],
                        function () {
                            var modalInstance = $modal.open({
                                templateUrl: "app/views/framework/security/addUser.html?id=1",
                                controller: 'AddEditUserController',
                                resolve: {
                                    inputParameter: function () {
                                        return mydata;
                                    }
                                }
                            });
                            modalInstance.result.then(function (selectedItem) {
                                var okPressed = $scope.SelectedOption;
                                if (okPressed == false) {
                                    return;
                                }
                                $scope.loadSecurityUsers();
                            }, function () {
                            });

                        });
        };

        $scope.editUser = function (user) {
            var mydata = {
                user: user
            };
            require(
                     [
                         "controllers/framework/security/AddEditUserController"
                     ],
                      function () {
                          var modalInstance = $modal.open({
                              templateUrl: 'app/views/framework/security/editUser.html',
                              controller: 'AddEditUserController',
                              resolve: {
                                  inputParameter: function () {
                                      return mydata;
                                  }
                              }
                          });
                      });
        };
        $scope.deleteUser = function (user) {
            var mydata = {
                message: "Are you sure you want to delete the " + user.Login + " user ?"
            };
            require(
                     [
                         "controllers/ConfirmationController"
                     ],
                      function () {
                          var modalInstance = $modal.open({
                              templateUrl: 'app/views/framework/confirmation.html?id=1',
                              controller: 'ConfirmationController',
                              resolve: {
                                  inputParameter: function () {
                                      return mydata;
                                  }
                              }
                          });

                          modalInstance.result.then(function (selectedItem) {
                              var okPressed = $scope.SelectedOption;
                              if (okPressed == false) {
                                  return;
                              }
                              var methodInfo =
                               {
                                   AssemblyName: "ESB.Security.DAL.dll",
                                   NameSpace: "ESB.Security.DAL.Security",
                                   MethodName: "SDeleteUser"
                               };
                              var esbRequest =
                                           {
                                               restUrl: "api/security/RemoveUser",
                                               correlationId: "",
                                               messageType: 1 | 4,
                                               methodInfo: methodInfo,
                                               jsonRequest: JSON.stringify(user),
                                           };
                              pusherService.PushESBMessage(esbRequest, $scope.OnHandleDeleteUserResponse, $scope);
                          }, function () {
                              //$log.info('Modal dismissed at: ' + new Date());
                          });
                      });

            
        };
        $scope.OnHandleDeleteUserResponse = function (obj, error) {

            if (obj.ResponseCode == null || obj.ResponseCode == '') {
                notificationService.ShowLocalizationMessage("User", obj.LocalizationCode, 1);
                //$scope.ServerMessage = obj.ResponseCode;
                $scope.loadSecurityUsers();
                return;
            }
            notificationService.ShowLocalizationMessage("User", obj.LocalizationCode);
            $scope.SecurityViewPath = "";
        };
        //note 2 -- End--------------------------------------------------------------------------------------------


        //note 3 -- below items moved to form ctr--------------------------------------------------------------------------------------------
        $scope.execNewUser = function () {
            $scope.user = $scope.newUser();
            //$scope.SecurityViewPath = "app/views/framework/security/newUser.html?id=1";
        };

        $scope.execUpdateUser = function () {
            var methodInfo =
                 {
                     AssemblyName: "ESB.Security.DAL.dll",
                     NameSpace: "ESB.Security.DAL.Security",
                     MethodName: "SUpdateUser"
                 };
            var esbRequest =
                         {
                             restUrl: "api/security/UpdateUser",
                             correlationId: "",
                             messageType: 1 | 4,
                             methodInfo: methodInfo,
                             jsonRequest: JSON.stringify($scope.user),
                         };
            pusherService.PushESBMessage(esbRequest, $scope.CreateUserReceived, $scope);

        };



        $scope.ServerMessage = "";


        //User End----

        $scope.executeFunction();
        $scope.$on('$destroy', function () {
            console.log($scope.ControllerName + ' is no longer necessary');
        })



        }]);

});

    


    