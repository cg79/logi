define(['app'], function(app) {
    'use strict';
    app.register.controller('CompanyController', ['$scope', '$modal', '$location', '$timeout', 'securityService', 'pusherService', 'esbSharedService', 'utils', 'notificationService',
        function($scope, $modal, $location, $timeout, securityService, pusherService, esbSharedService, utils, notificationService) {

            $scope.siglaObj = {
                sigla: null,
                siglaUrl: "",
                siglaGuid: utils.uuid()
            };

            var address = function() {
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

            var item = function() {
                var rez = {
                    text: ""
                };
                return rez;
            }

            var list = function(newItemFct) {
                var rez = {
                    first: null,
                    list: [],
                    addItem: function(it) {
                        if (it == undefined || it == null) {
                            it = new newItemFct();
                        }
                        this.list.push(it);
                    },
                    addItemNull: function() {
                        this.list.push(null);
                    },
                    removeItem: function(index, it) {
                        it = null;
                        this.list.splice(index, 1);
                    },
                    getList: function() {
                        return this.list;
                    },
                    setList: function(value) {
                        this.list = value;
                    }
                };
                return rez;
            };
            var cmpInfoObj = function() {
                this.UserGuid = esbSharedService.user.Guid;
                this.Guid = utils.uuidEmpty();
                this.NumeFirma = "";
                this.CIF = "";
                this.REG_Comert = "";
                this.SiglaUrl = null; 
                this.PuncteLucru = new list(item);
                this.inputAddr = "";
                this.Addresses = null;
                this.Phones = new list(item);
                this.Email = "";
                this.WebSite = "";
                this.ServerMessage = null;
                this.chooseSigla = function() {
                    angular.element("#siglafileupload").click();
                }
            }
            if ($scope.input == undefined) {
                $scope.input = {};
            }
            $scope.input.cmpInfo = new cmpInfoObj();

            $scope.$watch('input.cmpInfo.PuncteLucru.first', function() {
                var first = $scope.input.cmpInfo.PuncteLucru.first;
                if (first == undefined || first == null)
                    return;

                var addr = new address();
                addr.parseAddress(first.address_components);
                addr.location = first.geometry.location;

                $scope.input.cmpInfo.PuncteLucru.first = null;
                $scope.input.cmpInfo.inputAddr = "";


                var city = {
                    address: addr,
                    dist: 0
                };
                $scope.input.cmpInfo.PuncteLucru.addItem(city);
            });


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



            $scope.fileReaderSupported = window.FileReader != null && (window.FileAPI == null || FileAPI.html5 != false);



            $scope.generateSiglaThumb = function(file) {
                if (file != null) {
                    if ($scope.fileReaderSupported && file.type.indexOf('image') > -1) {
                        $timeout(function() {
                            var fileReader = new FileReader();
                            fileReader.readAsDataURL(file);
                            fileReader.onload = function(e) {
                                $timeout(function() {
                                    file.dataUrl = e.target.result;
                                    $scope.siglaObj.sigla = file;
                                    $scope.siglaObj.siglaUrl = file.dataUrl;
                                });
                            }
                        });
                    }
                }
            }


            $scope.showCompanyLocationsDialog = function() {
                require(
                    [
                        "controllers/company/CompanyLocationsController"
                    ],
                    function() {
                        var modalInstance = $modal.open({
                            templateUrl: 'app/controllers/company/companyLocations.html',
                            controller: 'CompanyLocationsController',
                            resolve: {
                                inputParameter: function() {
                                    var rez = $scope.input.cmpInfo.PuncteLucru.getList();

                                    return rez;
                                }
                            }
                        });
                        modalInstance.result.then(function(selectedItem) {
                            if (selectedItem == null || selectedItem == undefined)
                                return;

                            //$scope.$apply(function() {
                            $scope.input.cmpInfo.PuncteLucru.setList(selectedItem.way);
                            //});


                        }, function() {
                            //$log.info('Modal dismissed at: ' + new Date());
                        });
                    });
            };

            $scope.uploadSigla = function(files) {
                if ($scope.siglaObj.sigla == null) {
                    return;
                }
                var img = $scope.siglaObj.sigla;

                var data = new FormData();

                data.append($scope.siglaObj.siglaGuid, img);


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
                            //$scope.saveUserAvatar(data.name);
                            //$scope.input.cmpInfo.siglaUrl = data.name;
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

            $scope.saveCompany = function() {
                if (this.cmp.$invalid) {
                    //alert("invalid");
                    this.cmp.$setDirty();
                    return;
                }

                var mmm = {
                    Library: "UsersImplementation",
                    Namespace: "UsersImplementation.Repositories.CompanyRepository",
                    Method: "SaveCompany",
                    JSON: ""
                };
                $scope.input.cmpInfo.SiglaUrl = $scope.siglaObj.siglaGuid;


                mmm.JSON = JSON.stringify($scope.input.cmpInfo);

                pusherService.PushESBMessage(mmm, $scope.OnCompanySaved, $scope);
            };

            $scope.OnCompanySaved = function(obj) {
                //$scope.loadSecurityUsers();

                if (obj.R == null || obj.R == '') {
                    // $scope.$apply(function () {
                    $scope.DB_OP = $scope.ui.USER_SAVED;
                    $scope.uploadSigla();
                    return;
                    //});

                } else {
                    //$scope.$apply(function () {
                    console.log(obj.R);
                    $scope.input.cmpInfo.ServerMessage = $scope.ui[obj.R];
                    //});

                }
            };

            $scope.loadUserCompany =  function()
            {
                 var mmm = {
                    Library: "UsersImplementation",
                    Namespace: "UsersImplementation.Repositories.CompanyRepository",
                    Method: "GetCompanyByUserGuid",
                    JSON: ""
                };
                var request = {
                    userGuid : esbSharedService.user.Guid
                };

                mmm.JSON = JSON.stringify(request);

                pusherService.PushESBMessage(mmm, $scope.OnLoadUserCompany, $scope);   
            };

            $scope.OnLoadUserCompany =  function(obj)
            {
                  if(obj == null || obj == undefined)
                  return;

                $scope.input.cmpInfo = obj;

            };

            $scope.loadUserCompany();


        }
    ]);

});