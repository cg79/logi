define(['app'], function(app) {
    'use strict';

    app.register.controller('LoginController', ['$scope', '$timeout', '$location', 'pusherService', 'esbSharedService', 'securityService',
        function($scope, $timeout, $location, pusherService, esbSharedService, securityService) {
            $scope.input = {
                Login: "",
                Password: "",
                RememberMe: false,
                Email: "",
                ServerMessage: "",
                ResetEmail: "",
            };

            $scope.ui = null;
            var jqxhr = $.getJSON("app/localization/en/user.html", function(resp) {
                    $scope.$apply(function() {
                        $scope.ui = resp;
                    });

                })
                .fail(function(xxx) {
                    console.log("error loading car ui");
                });

            $scope.user = null;
            $scope.checkUser = function() {
                $scope.user = esbSharedService.user;
            }
            $scope.$on('onUserLogged', function() {
                $scope.checkUser();

            });
            $scope.checkUser();
            $scope.UILogin = function() {
                if (this.loginForm.$invalid) {
                    $scope.input.ServerMessage = $scope.ui.ID_FORM_INVALID;
                    return;
                }

                $scope.input.ServerMessage = "";
                var mmm = {
                    Library: "UsersImplementation",
                    Namespace: "UsersImplementation.Repositories.UsersRepository",
                    Method: "UILogin",
                    JSON: ""
                };
                mmm.JSON = JSON.stringify($scope.input);

                pusherService.PushESBMessage(mmm, $scope.OnUILogin, $scope);

            };

            $scope.navigateTo = function(val) {
                    $location.path(val).replace();
                }
                //$scope.LoggedScreenPath = "";

            $scope.OnUILogin = function(obj, error) {
                //$scope.loadSecurityUsers();

                if (obj.R == null || obj.R == '') {
                    // $scope.$apply(function () {
                    //$scope.input.ServerMessage = $scope.ui.USER_SAVED;
                    //});
                    //$scope.LoggedScreenPath = "app/user/loggedScreen.html";
                    esbSharedService.loggedIn(obj, $scope.input.RememberMe);
                    $location.path("/main").replace();
                } else {
                    //$scope.$apply(function () {
                    console.log(obj.R);
                    $scope.input.ServerMessage = $scope.ui[obj.R];
                    //});

                }
            };

            $scope.SendResetPasswordEmail = function() {

                if (this.frmReset.$invalid) {
                    //alert("invalid");
                    return;
                }

                $scope.input.ServerMessage = "";
                var mmm = {
                    Library: "UsersImplementation",
                    Namespace: "UsersImplementation.Repositories.UsersRepository",
                    Method: "UISendResetPassEmail",
                    JSON: ""
                };
                mmm.JSON = JSON.stringify($scope.input);

                pusherService.PushESBMessage(mmm, $scope.OnSendResetPasswordEmail, $scope);

            };

            $scope.OnSendResetPasswordEmail = function(obj, error) {
                //$scope.loadSecurityUsers();

                if (obj.R == null || obj.R == '') {
                    // $scope.$apply(function () {
                    $scope.input.ResetEmail = $scope.ui.RESET_EMAIL_SENT;
                    //});

                } else {
                    //$scope.$apply(function () {
                    console.log(obj.R);
                    $scope.input.ResetEmail = $scope.ui[obj.R];
                    //});

                }
            };

            $scope.GoToCreateUser = function() {
                $location.path("/user/create").replace();
            };
            $scope.GoToResetPassword = function() {
                debugger;
                $location.path("/user/resetpassword").replace();
            };

            $scope.fbLogin = function() {

            };
            $scope.logOut = function() {
                esbSharedService.logOut();
            };

            $scope.fileReaderSupported = window.FileReader != null && (window.FileAPI == null || FileAPI.html5 != false);
            $scope.avatar = null;
            $scope.avatarUrl = null;
            $scope.uploadPic = function(files) {
                if ($scope.avatar == null) {
                    return;
                }

                var data = new FormData();

                data.append("avatar", $scope.avatar);


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
                            $scope.setAvatar(data.name);
                        } else {
                            console.log('ERRORS: ' + data.error);
                        }
                    },
                    error: function(jqXHR, textStatus, errorThrown) {
                        // Handle errors here
                        console.log('ERRORS: ' + textStatus);
                        // STOP LOADING SPINNER
                    }
                });
            };
            $scope.chooseAvatar = function() {
                angular.element("#fileupload").click();
            };
            $scope.generateThumb = function(file) {
                if (file != null) {
                    if ($scope.fileReaderSupported && file.type.indexOf('image') > -1) {
                        $timeout(function() {
                            var fileReader = new FileReader();
                            fileReader.readAsDataURL(file);
                            fileReader.onload = function(e) {
                                $timeout(function() {
                                    file.dataUrl = e.target.result;
                                    $scope.avatar = file;
                                    $scope.user.uiImage = file.dataUrl;
                                    $scope.uploadPic();
                                });
                            }
                        });
                    }
                }
            }

            $scope.setAvatar = function(val) {

                var msg = {
                    g: $scope.user.Guid,
                    url: val
                };

                var mmm = {
                    Library: "UsersImplementation",
                    Namespace: "UsersImplementation.Repositories.UsersRepository",
                    Method: "UISetAvatar",
                    JSON: ""
                };
                mmm.JSON = JSON.stringify(msg);

                pusherService.PushESBMessage(mmm, $scope.OnSetAvatar, $scope);

            };
            $scope.OnSetAvatar = function(val) {
                debugger;
                if (val == undefined || val == null)
                    return;
                esbSharedService.user.ImgUrl = val.url;
                esbSharedService.updateLocalStorage();
            };



        }
    ]);
});