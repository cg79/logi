define(['app'], function(app) {
    'use strict';
    app.register.controller('CreateUserController', ['$scope', '$modal', '$location', '$timeout', '$upload', 'securityService', 'pusherService', 'esbSharedService', 'utils', 'notificationService',
        function($scope, $modal, $location, $timeout, $upload, securityService, pusherService, esbSharedService, utils, notificationService) {

            $scope.openDate = function($event) {
                $event.preventDefault();
                $event.stopPropagation();
                $scope.input.opened = true;
            };

            $scope.input = {
                Login: "",
                Phone: "",
                Password: "",
                CPassword: "",
                FirstName: "",
                LastName: "",
                Email: "",
                RememberMe: false,
                IsCompany: false,
                CompanyName: "",
                Sex: 1,
                BirthDay: null,

                opened: false,
                dateOptions: {
                    formatYear: 'yy',
                    startingDay: 1
                }
            };
            $scope.userImage = {
                avatar: "",
                avatarUrl: "",
                avatarGuid: utils.uuid()
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
                                    $scope.userImage.avatar = file;
                                    $scope.userImage.avatarUrl = file.dataUrl;
                                });
                            }
                        });
                    }
                }
            }

            $scope.uploadUserImage = function(files) {
                if ($scope.userImage.avatar == null) {
                    return;
                }

                var data = new FormData();

                data.append($scope.userImage.avatarGuid, $scope.userImage.avatar);


                $.ajax({
                    url: securityService.webroot + '/Home/UploadFiles?directory=Avatar',
                    type: 'POST',
                    data: data,
                    cache: false,
                    dataType: 'json',
                    processData: false, // Don't process the files
                    contentType: false, // Set content type to false as jQuery will tell the server its a query string request
                    success: function(data, textStatus, jqXHR) {
                        if (typeof data.error === 'undefined') {
                            // Success so call function to process the form
                            //$scope.saveUserAvatar(data.name);
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
            $scope.newUser = function() {
                if (this.frm.$invalid) {
                    //alert("invalid");
                    //return;
                }
                if ($scope.input.Password != $scope.input.CPassword) {
                    //this.frm.password.$setValidity("required", true);
                    this.frm.password.$setValidity("notmatch", false);
                    //return;
                }
                $scope.DB_OP = "";
                var mmm = {
                    Library: "UsersImplementation",
                    Namespace: "UsersImplementation.Repositories.UsersRepository",
                    Method: "NewUser",
                    JSON: ""
                };
                mmm.JSON = JSON.stringify($scope.input);

                pusherService.PushESBMessage(mmm, $scope.OnNewUser, $scope);

            };


            $scope.OnNewUser = function(obj) {
                //$scope.loadSecurityUsers();

                if (obj.R == null || obj.R == '') {
                    // $scope.$apply(function () {
                    $scope.DB_OP = $scope.ui.USER_SAVED;
                    $scope.uploadUserImage();
                   
                    return;
                    //});

                } else {
                    //$scope.$apply(function () {
                    console.log(obj.R);
                    $scope.DB_OP = $scope.ui[obj.R];
                    //});

                }
            };

            $scope.CompanyPath = "";
            $scope.viewCompany = function() {
                if ($scope.input.IsCompany) {
                    require(
                        [
                            "controllers/company/CompanyController"
                        ],
                        function() {
                            $scope.$apply(function() {
                                $scope.CompanyPath = "app/controllers/company/company.html";
                            });
                        });
                } else {
                    $scope.CompanyPath = "";
                }
            }



        }
    ]);

});