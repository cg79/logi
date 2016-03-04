define(['app'], function(app) {
    'use strict';
    app.register.controller('ProfilController', ['$scope', '$modal', '$location', '$timeout', '$upload', 'securityService', 'pusherService', 'esbSharedService', 'utils', 'notificationService',
        function($scope, $modal, $location, $timeout, $upload, securityService, pusherService, esbSharedService, utils, notificationService) {


            $scope.input = {
                Login: "",
                Phone: "",
               
                FirstName: "",
                LastName: "",
                Email: ""
            };
            $scope.input.Login = esbSharedService.user.Login;
            $scope.input.FirstName = esbSharedService.user.FirstName;
            $scope.input.LastName = esbSharedService.user.LastName;
            $scope.input.Email = esbSharedService.user.Email;
            $scope.input.Phone = esbSharedService.user.Phone;


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


            $scope.avatar = null;
            $scope.avatarUrl = null;

            $scope.$watch('files', function(files) {
                $scope.formUpload = false;
                if (files != null) {
                    for (var i = 0; i < files.length; i++) {
                        $scope.errorMsg = null;
                        (function(file) {
                            generateThumb(file);
                        })(files[i]);
                    }
                }
            });

            $scope.fileReaderSupported = window.FileReader != null && (window.FileAPI == null || FileAPI.html5 != false);
            $scope.picFile = [];
            $scope.$watch('files', function(files) {
                $scope.formUpload = false;
                if (files != null) {
                    for (var i = 0; i < files.length; i++) {
                        $scope.errorMsg = null;
                        (function(file) {
                            $scope.generateThumb(file);
                            eval($scope.uploadScript);
                        })(files[i]);
                    }
                }
                //storeS3UploadConfigInLocalStore();
            });

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
                                    $scope.avatarUrl = file.dataUrl;
                                    $scope.uploadPic();
                                });
                            }
                        });
                    }
                }
            }

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
            };

            $scope.DB_OP = "";
            $scope.updateProfilUser = function() {
                if (this.frm.$invalid) {
                    //alert("invalid");
                    return;
                }
                if ($scope.input.Password != $scope.input.CPassword) {
                    //this.frm.password.$setValidity("required", true);
                    this.frm.password.$setValidity("notmatch", false);
                    return;
                }
                $scope.DB_OP = "";
                var mmm = {
                    Library: "UsersImplementation",
                    Namespace: "UsersImplementation.Repositories.UsersRepository",
                    Method: "UpdateUser",
                    JSON: ""
                };
                var msg= {
                    Guid:esbSharedService.user.Guid,
                    //Email:$scope.input.Email,
                    FirstName:$scope.input.FirstName,
                    LastName:$scope.input.LastName,
                    Phone:$scope.input.Phone,
                    Login:$scope.input.Login

                };
                mmm.JSON = JSON.stringify(msg);

                pusherService.PushESBMessage(mmm, $scope.OnUserUpdated, $scope);

            };


            $scope.OnUserUpdated = function(obj, error) {
                //$scope.loadSecurityUsers();
                debugger;

                if (obj.R == null || obj.R == '') {
                    // $scope.$apply(function () {
                    $scope.DB_OP = $scope.ui.USER_SAVED;
                    //});

                } else {
                    //$scope.$apply(function () {
                    console.log(obj.R);
                    $scope.DB_OP = $scope.ui[obj.R];
                    //});

                }
            };



        }
    ]);

});