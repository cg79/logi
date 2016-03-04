define(['app'], function(app) {
    'use strict';

    app.register.controller('ForgotPasswordController', ['$scope',  '$location', 'pusherService', 'esbSharedService',
        function($scope, $location, pusherService, esbSharedService) {

            $scope.input = {
                Login: "",
                Password: "",
                RememberMe: false,
                Email: "",
                ServerMessage: "",
                ResetEmail: "",
            };

            // $scope.ui = null;
            // var jqxhr = $.getJSON("app/localization/en/user.html", function(resp) {
            //         $scope.$apply(function() {
            //             $scope.ui = resp;
            //         });

            //     })
            //     .fail(function(xxx) {
            //         console.log("error loading car ui");
            //     });


          

            $scope.navigateTo= function(val)
            {
               $location.path(val).replace();
            }
            //$scope.LoggedScreenPath = "";

           

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
                $location.path("/user/resetpassword").replace();
            };

            



        }
    ]);
});