define(['app'], function(app) {
    'use strict';

    app.controller('AuthController', ['$scope', '$modal', '$timeout', '$location', 'securityService', 'pusherService', 'esbSharedService',
        function($scope, $modal, $timeout, $location, securityService, pusherService, esbSharedService) {
            $scope.user = {
                Login: "",
                Password: "",
                RememberMe: false,
                Email: "",
                ServerError: ""
            };

            $scope.userAvatar = {
                img: null,
                imgUrl: "",
                setImage:function(val)
                {
                    this.img = val;
                    this.imgUrl = val.dataUrl;
                    $scope.LoggedUser.Avatar = val.dataUrl;
                }
            };

            $scope.useSignalR = false;
            $scope.useSignalRChanged = function() {
                pusherService.useSignalRChanged();
            }

            $scope.LoggedUser = securityService.LoggedUser;
            if ($scope.LoggedUser.Avatar == "") {
                $scope.LoggedUser.Avatar = "content/images/no-photo.jpg";
            }

            this.modalLoginForm = null;
            // $scope.$on('handleBroadcast', function() {

            //     $scope.LoggedUser = esbSharedService.LoggedUser;
            //     if (this.modalLoginForm != undefined && this.modalLoginForm != null) {
            //         this.modalLoginForm.close();
            //         this.modalLoginForm = null;
            //     }
            // });


            $scope.$on('onUserLoggedOut', function() {
                //$scope.$apply(function () {
                $scope.LoggedUser = securityService.LoggedUser;
                //});
            });

            $scope.localization = esbSharedService.localization;
            $scope.$on('onLocalizationReceived', function() {
                //$scope.$apply(function () {
                $scope.localization = esbSharedService.localization;
                //$scope.permissions = securityService.permissions;
                //});
            });



            //note1----------------------------------------------bellow will be refactored/removed

            $scope.navigateToResetPasswordScreen = function() {
                $location.path("/password").replace();
            }

            $scope.showUserProfileForm = function() {

                $scope.execLoadUserProfile();
            }
            $scope.execLoadUserProfile = function() {
                var methodInfo = {
                    AssemblyName: "ESB.Security.DAL.dll",
                    NameSpace: "ESB.Security.DAL.Security",
                    MethodName: "SGetUserProfile"
                };
                var esbRequest = {
                    restUrl: "api/security/SGetUserProfile",
                    correlationId: "",
                    messageType: 1 | 4,
                    methodInfo: methodInfo,
                    jsonRequest: JSON.stringify(securityService.LoggedUser),
                };
                pusherService.PushESBMessage(esbRequest, $scope.ChangeUserProfileReceived, $scope);

            }
            $scope.ChangeUserProfileReceived = function(obj, error) {

                    $scope.user = obj.User;
                    //$scope.user.Email = obj.User.Email;

                    var mydata = {
                        user: obj.User
                    };
                    require(
                        [
                            "controllers/framework/security/AddEditUserController"
                        ],
                        function() {
                            var modalInstance = $modal.open({
                                templateUrl: securityService.webroot + '/app/views/framework/user/editUserForm.html',
                                controller: 'AddEditUserController',
                                resolve: {
                                    inputParameter: function() {
                                        return mydata;
                                    },
                                    //aaa: function () {
                                    //    return 'aaaaa';
                                    //}
                                }
                            });


                        });
                }
                //note1---------------------------



            $scope.showCreateUserForm = function() {

                var mydata = {
                    formName: securityService.webroot + "app/views/framework/user/createUserForm.html",
                };
                require(
                    [
                        "controllers/framework/security/AddEditUserController"
                    ],
                    function() {
                        var modalInstance = $modal.open({
                            templateUrl: securityService.webroot + 'app/views/framework/user/signIn.html',
                            controller: 'LoginController',
                            resolve: {
                                inputParameter: function() {
                                    return mydata;
                                }
                            }
                        });
                    });
            }



            $scope.logout = function() {
                securityService.ResetLoggedUser();
                esbSharedService.logOut();
                var appScope = angular.element("#esbBody").scope();
                appScope.LoginScreen = "";

                $location.path("/auth");
            }

            $scope.PermissionsAreEnabled = false;
            $scope.enablePermission = function() {
                $scope.PermissionsAreEnabled = true;
                esbSharedService.enablePermissions(true);
            }
            $scope.disablePermission = function() {
                $scope.PermissionsAreEnabled = false;
                esbSharedService.enablePermissions(false);
            }

            $scope.execLogin = function() {
                if (this.loginForm.$invalid) {
                    //alert("invalid");
                    return;
                }
                this.loginForm.$setPristine();
                var methodInfo = {
                    AssemblyName: "ESB.Security.DAL.dll",
                    NameSpace: "ESB.Security.DAL.Security",
                    MethodName: "SLogin"
                };
                var esbRequest = {
                    restUrl: "api/security/login",
                    correlationId: "",
                    messageType: 1 | 4,
                    methodInfo: methodInfo,
                    jsonRequest: JSON.stringify($scope.user),
                    //jsonRequest: $scope.user,
                };

                pusherService.PushESBMessage(esbRequest, $scope.LoginReceived, $scope);

            }

            $scope.LoginReceived = function(obj, error) {

                if (obj.ResponseCode != null && obj.ResponseCode != '') {
                    // $scope.$apply(function () {
                    $scope.user.ServerError = "Invalid user or password";
                    $scope.LoggedUser.ServerError = $scope.user.ServerError;
                    // });
                    return;
                }

                //preceed other condition until proper result code is returned (simulate as succesfull when null)
                securityService.SetLoggedUser(obj.User.ID, obj.User.Login, obj.User.FirstName, obj.User.LastName, obj.User.Email, obj.User.UserGuid, false, obj.User.Avatar);

                $scope.LoggedUser = securityService.LoggedUser;
                //$scope.LoggedUser = securityService.LoggedUser;
                //securityService.GetLoggedUser();

                $scope.user.Login = '';
                $scope.user.Password = '';
                $scope.user.ServerError = '';
                $scope.LoggedUser.Avatar = obj.User.Avatar;
                securityService.securityToken = obj.UserPermissions.SecurityToken;
                if ($scope.user.RememberMe == true) {
                    securityService.RememberUser(true);
                } else {
                    //remove user from local storage:
                    securityService.RememberUser(false);
                }
                securityService.permissions = obj.UserPermissions;
                esbSharedService.permissionsChanged();

                var appScope = angular.element("#esbBody").scope();
                appScope.LoginScreen = "app/views/framework/auth/loggedScreen.html";

                if (obj.UserPermissions.Roles[0].Name == "SuperAdmin")
                { $location.path("/management"); }
                else if (obj.UserPermissions.Roles[0].Name == "Admin")
                { $location.path("/entrydata"); }
                else if (obj.UserPermissions.Roles[0].Name == "ViewOnly")
                { $location.path("/dashboard"); }
            }

            $scope.$on('onPermissionsChanged', function() {
                //$scope.$apply(function () {
                $scope.permissions = securityService.permissions;
                var temp = _.filter(securityService.permissions.Roles,
                    function(o) {
                        return o.Name == "Admin";
                    });

                if (temp.length == 0) {
                    $scope.PermissionsAreEnabled = false;
                }
                //});
            });

            $scope.esbLogin = function(esbRequest) {
                var call = pusherService.PushESBMessageWithREST(esbRequest, $scope.LoginReceived);
                call.then(function(val) {
                    var receivedObj = JSON.parse(val);
                    var jsonResponse = null;
                    var error = null;

                    if (receivedObj.Error == null) {
                        jsonResponse = JSON.parse(receivedObj.JsonResponse);
                    } else {
                        error = receivedObj.Error;
                    }

                    $scope.LoginReceived(jsonResponse, error);
                });
            }

            $scope.execADLogin = function() {
                var methodInfo = {
                    AssemblyName: "ESB.Security.DAL.dll",
                    NameSpace: "ESB.Security.DAL.Security",
                    MethodName: "ADLogin"
                };
                var esbRequest = {
                    correlationId: "",
                    messageType: 1 | 4,
                    methodInfo: methodInfo,
                    jsonRequest: JSON.stringify($scope.user),
                };
                pusherService.PushESBMessage(esbRequest, $scope.ADLoginReceived, $scope);
            }

            $scope.ADLoginReceived = function(obj, error) {

                if (obj.IsAuthorized == true) { //preceed other condition until proper result code is returned (simulate as succesfull when null)
                    securityService.SetLoggedUser(-1, obj.Name, "AD_User", false);
                    securityService.LoggedUser.LoggedMode = "AD";

                   // $scope.LoggedUser = securityService.LoggedUser;
                    //securityService.GetLoggedUser();

                    $scope.user.Login = '';
                    $scope.user.Password = '';
                    $scope.user.ServerError = '';
                    if ($scope.user.RememberMe == true) {
                        securityService.RememberUser(true);
                    } else {
                        //remove user from local storage:
                        securityService.RememberUser(false);
                    }

                    return;
                } else {
                    $scope.user.ServerError = "Invalid user or password";
                    $scope.LoggedUser.ServerError = $scope.user.ServerError;
                }


            }

        
            $scope.chooseAvatar = function() {
                angular.element("#avatarfileupload").click();
            };

            $scope.fileReaderSupported = window.FileReader != null && (window.FileAPI == null || FileAPI.html5 != false);
           
            $scope.generateAvatarThumb = function(file) {
               if (file == null) {
                    return;
                }

                if ($scope.fileReaderSupported && file.type.indexOf('image') > -1) {
                        $timeout(function() {
                            var fileReader = new FileReader();
                            fileReader.readAsDataURL(file);
                            fileReader.onload = function(e) {
                                $timeout(function() {
                                    file.dataUrl = e.target.result;
                                    $scope.userAvatar.setImage(file);
                                    $scope.uploadPic();
                                });
                            }
                        });
                    }
            }

            $scope.saveUserAvatar = function(filePath) {
                var methodInfo = {
                    AssemblyName: "ESB.Security.DAL.dll",
                    NameSpace: "ESB.Security.DAL.Security",
                    MethodName: "SSetUserAvatar"
                };
                var img = {
                    name: "App_Data/Avatar/" + filePath,
                    userid: $scope.LoggedUser.ID
                };
                var esbRequest = {
                    restUrl: "api/security/SetUserAvatar",
                    correlationId: "",
                    messageType: 1 | 4,
                    methodInfo: methodInfo,
                    jsonRequest: JSON.stringify(img),
                };
                pusherService.PushESBMessage(esbRequest, $scope.AvatarReceived, $scope);
            }

            $scope.AvatarReceived = function(obj, error) {
                if (obj.ResponseCode != null) {

                    return;
                }
                securityService.LoggedUser.Avatar = obj.filePath;
                securityService.UpdateUserAvatar();
            };

            $scope.uploadPic = function(files) {
                    if ($scope.userAvatar.imgUrl == null) {
                        return;
                    }

                    var data = new FormData();

                    data.append("avatar", $scope.userAvatar.img);


                    $.ajax({
                        url: securityService.webroot + '/Home/UploadFiles',
                        type: 'POST',
                        data: data,
                        cache: false,
                        dataType: 'json',
                        processData: false, // Don't process the files
                        contentType: false, // Set content type to false as jQuery will tell the server its a query string request
                        success: function(data, textStatus, jqXHR) {
                            if (typeof data.error === 'undefined') {
                                // Success so call function to process the form
                                $scope.saveUserAvatar(data.name);
                            } else {
                                // Handle errors here
                                console.log('ERRORS: ' + data.error);
                            }
                        },
                        error: function(jqXHR, textStatus, errorThrown) {
                            // Handle errors here
                            console.log('ERRORS: ' + textStatus);
                            // STOP LOADING SPINNER
                        }
                    });



                }
                //avatar
        }
    ]);


});