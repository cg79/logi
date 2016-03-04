//var app = angular.module('esb_api', ['ui.bootstrap', 'ngGrid', 'smart-table', 'LocalStorageModule', 'ui.utils', 'ngResource', 'angularFileUpload']);
define(['app'], function(app) {
    'use strict';



    app.controller('AppController', ['$scope', '$rootScope', '$location', '$resource', '$modal', 'pusherService', 'utils', 'esbSharedService', 'securityService',
        function($scope, $rootScope, $location, $resource, $modal, pusherService, utils, esbSharedService, securityService) {

moment.locale('ro');
            securityService.webroot = utils.getRootWebSitePath();
            $scope.ControllerName = "AppController";
            $scope.LoginScreen = "";
            $scope.isVisible = false;
            $scope.myColor = "magenta";


            $scope.ShowUserControls = false;
            $scope.LoggedUser = securityService.LoggedUser;


            $rootScope.$on('$routeChangeStart', function(event, next, current) {
                if (next.$$route == undefined || next.$$route.secure == undefined)
                    return;
                var isSecure = next.$$route.secure;
                if (isSecure) {
                    if (esbSharedService.user != null) {
                        return;
                    }
                    //$location.path("/login").replace();
                    event.preventDefault();
                    $rootScope.$evalAsync(function() {
                        $location.path('/login');
                    });
                }
            });

            $scope.PermissionsReceived = function(obj, error) {
                securityService.securityToken = obj.UserPermissions.SecurityToken;
                securityService.permissions = obj.UserPermissions;
                esbSharedService.permissionsChanged();
            };



            if (securityService.LoggedUser != null && securityService.LoggedUser != undefined && securityService.LoggedUser.ID > 0) {
                $scope.LoginScreen = "app/views/auth/loggedScreen.html";
                //$scope.getUserPermissions();
            } else {
                $scope.LoginScreen = "app/views/auth/auth.html";
            }

            $scope.$on('onSignalRConnectionEstablished', function() {
                if (esbSharedService.isSignalRConnection == false)
                    return;

                if (securityService.LoggedUser != null && securityService.LoggedUser != undefined && securityService.LoggedUser.ID > 0) {
                    // $scope.getUserPermissions();
                }
            });

            $scope.navigateTo = function(loc) {
                $location.path(loc).replace();
            }

            $scope.isPermissionsEnabled = esbSharedService.isPermissionsEnabled;
            $scope.$on('onShowHidePermissions', function() {
                $scope.isPermissionsEnabled = esbSharedService.isPermissionsEnabled;
            });

            $scope.ui = null;
            $scope.getLocalization = function() {

                //var dataService = 
                //$resource('localization.html', {}, {
                //    getData: { method: 'GET', isArray: false }
                //});

                var jqxhr = $.getJSON("app/localization/en/localization.html", function(resp) {
                        $scope.ui = resp;
                    })
                    .fail(function(xxx) {
                        console.log("error loading localization");

                    });


                //$http.get('localization.config')
                //.success(function (data) {
                //    var locData = JSON.parse(data);
                //    esbSharedService.localizationReceived(locData);
                //});
                //var responsePromise = $http.get("app/localization/en/localization.json");

                //responsePromise.success(function (data, status, headers, config) {
                //    data = "{" + data + "}";
                //    var locData = JSON.parse(data);
                //    esbSharedService.localizationReceived(locData);
                //});
                //responsePromise.error(function (data, status, headers, config) {
                //    alert("AJAX failed!");
                //});
            }
            $scope.getLocalization();

            $scope.$on('onPermissionsChanged', function() {
                //$scope.$apply(function () {
                $scope.permissions = securityService.permissions;
                if (securityService.LoggedUser != null && securityService.LoggedUser != undefined) {
                    $scope.ShowUserControls = securityService.LoggedUser.ID > 0;
                }

                //});
            });

            $scope.$on('onUserLoggedOut', function() {
                //$scope.$apply(function () {
                $scope.ShowUserControls = false;
                //$scope.permissions = securityService.permissions;
                //});
            });

            $scope.lsp = "";
            $scope.user = null;
            $scope.checkUser = function() {
                $scope.user = esbSharedService.user;

                require(
                    [
                        "controllers/user/LoginController"
                    ],
                    function() {
                        $scope.$apply(function() {
                            if (esbSharedService.user == null) {
                                $scope.lsp = "app/controllers/user/login.html";
                            } else {
                                $scope.lsp = "app/controllers/user/loggedScreen.html";
                            }
                        });
                    }
                );
            }


            $scope.$on('onUserLogged', function() {
                $scope.checkUser();
            });

            // login screen path



            $scope.checkUser();

            $scope.response = "";

            $scope.LeftMenu = "app/controllers/menu/menu.html?id='" + utils.uuid() + "'";
            $scope.showHtml = function(filePath, ctrlName, scopeVariable, renderMode) {
                var mydata = {
                    filePath: filePath,
                    ctrlName: ctrlName,
                    scopeVariable: scopeVariable,
                    renderMode: renderMode
                };
                require(
                    [
                        "controllers/security/AddEditUserController"
                    ],
                    function() {
                        var modalInstance = $modal.open({
                            templateUrl: 'app/views/editor/ace/aceEditorHtml.html?id=2',
                            controller: 'HtmlController',
                            size: 'lg',
                            resolve: {
                                inputParameter: function() {
                                    return mydata;
                                }
                            }
                        });
                    });
            }
        }
    ]);

    app.directive('passwordValidate', function() {
        return {
            require: 'ngModel',
            link: function(scope, elm, attrs, ctrl) {
                ctrl.$parsers.unshift(function(viewValue) {

                    scope.pwdValidLength = (viewValue && viewValue.length >= 8 ? 'valid' : undefined);
                    scope.pwdHasLetter = (viewValue && /[A-z]/.test(viewValue)) ? 'valid' : undefined;
                    scope.pwdHasNumber = (viewValue && /\d/.test(viewValue)) ? 'valid' : undefined;

                    if (scope.pwdValidLength && scope.pwdHasLetter && scope.pwdHasNumber) {
                        ctrl.$setValidity('pwd', true);
                        return viewValue;
                    } else {
                        ctrl.$setValidity('pwd', false);
                        return undefined;
                    }
                });
            }
        };
    });

    app.directive('hasPermission', function() {
        return {
            scope: {
                permissions: "=",
            },
            link: function(scope, element, attrs) {
                if (scope.permissions == undefined || scope.permissions == null)
                    return;
                var fName = attrs.functionalityName;

                var temp = _.filter(scope.permissions.UserPermissions,
                    function(o) {
                        return o.f == fName;
                    });

                if (temp.length > 0) {
                    switch (temp[0].h) {
                        case 1:
                            {
                                element.hide();
                                break;
                            }
                        case 2:
                            {
                                element.attr("disabled", "disabled");
                                break;
                            }
                        case 3:
                            {
                                element.attr("readonly", "readonly");
                                break;
                            }
                    }
                }
            }
        }
    });

    app.directive('hideIfNotAdmin', function(securityService) {
        return {

            link: function(scope, element, attrs) {



                var temp = _.filter(securityService.permissions.Roles,
                    function(o) {
                        return o.Name == "Admin";
                    });

                if (temp.length == 0) {
                    element.hide();
                }
            }
        }
    });

    app.directive('myIsolatedScopeWithName', function($compile) {
        return {
            replace: true,
            scope: {
                functionalityName: '@',
                isPermissionsEnabled: '@',
                permissions: '=',
                secTitle: '@',
                onClick: "&",
                onSecurityClick: '&'
            },
            template: '<ng-include src="getTemplateUrl()"/>',
            // templateUrl: "app/templates/sec_button.html",
            //template: 'Name: {{ name }} {{isPermissionsEnabled}}'
            controller: function($scope) {
                //function used on the ng-include to resolve the template
                $scope.getTemplateUrl = function() {
                    //basic handling
                    if ($scope.isPermissionsEnabled == "true")
                        return "app/templates/sec_button.html";

                    return "app/templates/no_sec_button.html";
                }
                $scope.call = function() {
                    //basic handling
                    $scope.onClick();
                }
                $scope.callSecurity = function() {
                    //basic handling
                    $scope.onSecurityClick();
                }
            }
        };
    });

    app.directive('jqPager', function(utils) {
        return {
            replace: false,
            scope: {
                totalItems: '=',
                itemsPerPage: '=',
                pageIndex: '=',


            },
            templateUrl: 'app/templates/jqPager.html',
            // templateUrl: "app/templates/sec_button.html",
            //template: 'Name: {{ name }} {{isPermissionsEnabled}}'
            link: function(scope, element, attrs) {

                scope.rows = [5, 10, 25];
                scope.totalPages = 0;
                scope.$watch('totalItems', function() {
                    var totalPages = scope.itemsPerPage < 1 ? 1 : Math.ceil(scope.totalItems / scope.itemsPerPage);
                    scope.totalPages = Math.max(totalPages || 0, 1);
                });

                scope.$watch('itemsPerPage', function() {
                    var totalPages = scope.itemsPerPage < 1 ? 1 : Math.ceil(scope.totalItems / scope.itemsPerPage);
                    scope.totalPages = Math.max(totalPages || 0, 1);
                });
                scope.setPageIndex = function(newV) {
                    //scope.$apply(function () {
                    scope.pageIndex = newV;
                    //});
                }
                scope.noPrevious = function() {
                    return scope.pageIndex === 1;
                };
                scope.noNext = function() {
                    return scope.pageIndex === scope.totalPages;
                };
                scope.next = function() {
                    var last = scope.pageIndex === scope.totalPages;
                    if (last) {
                        return;
                    }
                    scope.setPageIndex(scope.pageIndex + 1);

                }
                scope.previous = function() {
                    var prev = scope.pageIndex === 1;
                    if (prev) {
                        return;
                    }
                    scope.setPageIndex(scope.pageIndex - 1);
                }
                scope.first = function() {
                    scope.setPageIndex(1);
                }
                scope.last = function() {
                    scope.setPageIndex(scope.totalPages);
                }
                scope.changePageIndex = function(newPageIndex) {
                    var newv = utils.ConvertToInt(newPageIndex);

                    if (newv == null || newv == undefined)
                        return;
                    if (newv > scope.totalpages) {
                        return;
                    }
                    if (newv < 1) {
                        return;
                    }
                    scope.setPageIndex(newv);
                }
            }
        };
    });

    app.directive('customFilter', function(utils) {
        return {
            replace: false,
            scope: {
                gridColumns: '=',
                execFilter: '&'
            },
            templateUrl: 'app/templates/stringFilter.html',
            // templateUrl: "app/templates/sec_button.html",
            //template: 'Name: {{ name }} {{isPermissionsEnabled}}'
            link: function(scope, element, attrs) {

                scope.createStringObj = function() {
                    this.criterias = [];
                    this.filterOptionID = 0;
                    this.inputValue = "";
                    this.init = function() {
                        this.criterias.push({
                            Index: 0,
                            OpID: 0,
                            Name: "Is Equal To"
                        });
                        this.criterias.push({
                            Index: 1,
                            OpID: 6,
                            Name: "Is Not Equal To"
                        });
                        this.criterias.push({
                            Index: 2,
                            OpID: 7,
                            Name: "Starts With"
                        });
                        this.criterias.push({
                            Index: 3,
                            OpID: 8,
                            Name: "Contains"
                        });
                        this.criterias.push({
                            Index: 4,
                            OpID: 9,
                            Name: "Does Not Contain"
                        });
                        this.criterias.push({
                            Index: 5,
                            OpID: 10,
                            Name: "Ends With"
                        });
                    }
                }

                for (var i = 0; i < scope.gridColumns.length; i++) {
                    scope.gridColumns[i].Filter = new scope.createStringObj();
                    scope.gridColumns[i].Filter.init();
                }
                scope.filter = function() {
                    scope.execFilter();
                };


            }
        };
    });

    app.directive('genericFilter', function($timeout, utils) {
        return {
            replace: false,
            scope: {
                gridColumns: '=',
                searchText: '=',
                execFilter: '&'
            },
            templateUrl: 'app/templates/genericFilter.html',
            // templateUrl: "app/templates/sec_button.html",
            //template: 'Name: {{ name }} {{isPermissionsEnabled}}'
            link: function(scope, element, attrs) {

                scope.filter = function() {
                    scope.execFilter();
                };
                scope.clearSearchText = function() {
                    scope.searchText = "";
                }
                scope.clearFilters = function() {

                    for (var i = 0; i < scope.gridColumns.length; i++) {
                        scope.gridColumns[i].Selected = false;
                    }
                    scope.execFilter();
                };
                var promise = null;
                scope.$watch('searchText', function(newValue, oldValue) {
                    if (newValue == oldValue) {
                        return;
                    }

                    if (promise !== null) {
                        $timeout.cancel(promise);
                    }
                    promise = $timeout(function() {
                        //tableCtrl.search(evt.target.value, scope.predicate || '');
                        scope.execFilter();
                        promise = null;
                    }, 500);
                });

            }
        };
    });

    app.directive('samep', function() {
        return {
            require: 'ngModel',
            scope: {
                p1: '=',
                p2: '=',
            },
            link: function(scope, element, attrs, ctrl) {


                scope.$watch('p1', function(newv, oldValue) {
                    if (newv == null || newv == undefined || newv == "")
                        return;
                    if (scope.p1 == scope.p2) {
                        ctrl.$setValidity('same', true);

                    } else {
                        ctrl.$setValidity('same', false);
                    }
                });
                scope.$watch('p2', function(newv, oldValue) {
                    if (newv == null || newv == undefined || newv == "")
                        return;
                    if (scope.p1 == scope.p2) {
                        ctrl.$setValidity('same', true);

                    } else {
                        ctrl.$setValidity('same', false);
                    }
                });
            }
        };
    });

    app.directive('pwCheck', [function() {
        return {
            require: 'ngModel',
            link: function(scope, elem, attrs, ctrl) {
                var firstPassword = '#' + attrs.pwCheck;
                elem.add(firstPassword).on('keyup', function() {
                    scope.$apply(function() {
                        var v = elem.val() === $(firstPassword).val();
                        ctrl.$setValidity('pwmatch', v);
                    });
                });
            }
        }
    }]);

    app.directive('dynamicElement', ['$compile', function($compile) {
        return {
            restrict: 'E',
            scope: {
                message: "="
            },
            replace: true,
            link: function(scope, element, attrs) {
                scope.$watch("message", function() {
                    var template = $compile(scope.message)(scope);
                    //element.replaceWith(template);  

                    element.html("");

                    // add the template content
                    element.append(template);
                    //element.append( template);
                });
            }
        }
    }]);

    app.directive('googlePlaces', function() {
        return {

            // transclude:true,
            scope: {
                location: '='
            },
            //template: '<input id="google_places_ac" name="google_places_ac" type="text" class="input-block-level"/>',
            link: function($scope, elm, attrs) {

                var autocomplete = new google.maps.places.Autocomplete(elm[0], {});
                google.maps.event.addListener(autocomplete, 'place_changed', function() {
                    var place = autocomplete.getPlace();
                    //$scope.location = place.geometry.location; //.location.lat() + ',' + place.geometry.location.lng();
                    $scope.location = place;
                    $scope.$apply();
                });
            }
        }
    });

    app.directive('googleplace', function() {
    return {
         // <div><input ng-model="chosenPlace" googleplace/></div>
        require: 'ngModel',
        link: function(scope, element, attrs, model) {
            var options = {
                types: [],
                componentRestrictions: {}
            };
            scope.gPlace = new google.maps.places.Autocomplete(element[0], options);
 
            google.maps.event.addListener(scope.gPlace, 'place_changed', function() {
                scope.$apply(function() {
                    model.$setViewValue(element.val());                
                });
            });
        }
    };
});

    app.directive('numbersOnly', function() {
        return {
            require: 'ngModel',
            link: function(scope, element, attrs, modelCtrl) {
                modelCtrl.$parsers.push(function(inputValue) {
                    // this next if is necessary for when using ng-required on your input. 
                    // In such cases, when a letter is typed first, this parser will be called
                    // again, and the 2nd time, the value will be undefined
                    if (inputValue == undefined) return ''
                    var transformedInput = inputValue.replace(/[^0-9]/g, '');
                    if (transformedInput != inputValue) {
                        modelCtrl.$setViewValue(transformedInput);
                        modelCtrl.$render();
                    }

                    return transformedInput;
                });
            }
        };
    });

    app.directive('same', function() {
        return {
            require: 'ngModel',
            link: function(scope, elem, attrs, ngModel) {
                ngModel.$parsers.unshift(validate);

                // Force-trigger the parsing pipeline.
                scope.$watch(attrs.same, function() {
                    ngModel.$setViewValue(ngModel.$viewValue);
                });

                function validate(value) {
                    var isValid = scope.$eval(attrs.same) == value;

                    ngModel.$setValidity('same', isValid);

                    return isValid ? value : undefined;
                }
            }
        };
    });

    function isEmpty(value) {
        return angular.isUndefined(value) || value === '' || value === null || value !== value;
    }
    app.directive('ngMin', function() {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function(scope, elem, attr, ctrl) {
                scope.$watch(attr.ngMin, function() {
                    ctrl.$setViewValue(ctrl.$viewValue);
                });
                var minValidator = function(value) {
                    var min = scope.$eval(attr.ngMin) || 0;
                    if (!isEmpty(value) && value < min) {
                        ctrl.$setValidity('ngMin', false);
                        return undefined;
                    } else {
                        ctrl.$setValidity('ngMin', true);
                        return value;
                    }
                };

                ctrl.$parsers.push(minValidator);
                ctrl.$formatters.push(minValidator);
            }
        };
    });

    app.directive('ngMax', function() {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function(scope, elem, attr, ctrl) {
                scope.$watch(attr.ngMax, function() {
                    ctrl.$setViewValue(ctrl.$viewValue);
                });
                var maxValidator = function(value) {
                    var max = scope.$eval(attr.ngMax) || Infinity;
                    if (!isEmpty(value) && value > max) {
                        ctrl.$setValidity('ngMax', false);
                        return undefined;

                        //  ctrl.$setViewValue(max);
                        // ctrl.$render();
                    } else {
                        ctrl.$setValidity('ngMax', true);
                        return value;
                    }
                };

                ctrl.$parsers.push(maxValidator);
                ctrl.$formatters.push(maxValidator);
            }
        };
    });

    app.filter('ctime', function() {
        return function(jsonDate) {
            var date = new Date(parseInt(jsonDate.substr(6)));
            return date;
        };
    });

    app.filter('propsFilter', function() {
        return function(items, props) {
            var out = [];

            if (angular.isArray(items)) {
                items.forEach(function(item) {
                    var itemMatches = false;

                    var keys = Object.keys(props);
                    for (var i = 0; i < keys.length; i++) {
                        var prop = keys[i];
                        var text = props[prop].toLowerCase();
                        if (item[prop].toString().toLowerCase().indexOf(text) !== -1) {
                            itemMatches = true;
                            break;
                        }
                    }

                    if (itemMatches) {
                        out.push(item);
                    }
                });
            } else {
                // Let the output be the input untouched
                out = items;
            }

            return out;
        }
    });
    app.filter('tel', function() {
        return function(phoneNumber) {
            if (!phoneNumber)
                return phoneNumber;



            return formatLocal('US', phoneNumber);
        }
    });
    app.filter('mom', function() {
        return function(dateString) {
            return moment(dateString).format('LL');
        };
    });

     app.filter('fromNow', function() {
        return function(dateString) {
            return moment(dateString).fromNow();
        };
    });

});