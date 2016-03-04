define(['app'], function(app) {
    'use strict';
    app.register.controller('ChangePasswordController', ['$scope', '$modal', '$location', '$timeout', '$upload', 'securityService', 'pusherService', 'esbSharedService', 'utils', 'notificationService',
        function($scope, $modal, $location, $timeout, $upload, securityService, pusherService, esbSharedService, utils, notificationService) {

            var inputObj = function() {
                this.OldP = "";
                this.Password = "";
                this.CPassword = "";
                this.ServerMessage = "";
                this.same = false;
            }

            $scope.input = new inputObj();


            $scope.ui = null;
            var jqxhr = $.getJSON("app/localization/en/user.html", function(resp) {
                    $scope.$apply(function() {
                        $scope.ui = resp;
                    });

                })
                .fail(function(xxx) {
                    console.log("error loading car ui");
                });


           
            $scope.changePassword = function() {
                if (this.frm.$invalid) {
                    //alert("invalid");
                    return;
                }
                if ($scope.input.Password != $scope.input.CPassword) {
                    //this.frm.password.$setValidity("required", true);
                    this.frm.password.$setValidity("notmatch", false);
                    return;
                }
                var mmm = {
                    Library: "UsersImplementation",
                    Namespace: "UsersImplementation.Repositories.UsersRepository",
                    Method: "UIChangePassword",
                    JSON: ""
                };
                var msg = {
                    g:esbSharedService.user.Guid,
                    OldP:$scope.input.OldP,
                    Password:$scope.input.Password
                };
                mmm.JSON = JSON.stringify(msg);

                pusherService.PushESBMessage(mmm, $scope.OnChangePassword, $scope);

            };


            $scope.OnChangePassword = function(obj, error) {
                //$scope.loadSecurityUsers();

                if (obj == null || obj.R == null || obj.R == '') {
                    // $scope.$apply(function () {
                    $scope.input.ServerMessage = $scope.ui.USER_SAVED;
                    //});

                } else {
                    //$scope.$apply(function () {
                    console.log(obj.R);
                    $scope.input.ServerMessage = $scope.ui[obj.R];
                    //});

                }
            };



        }
    ]);

});