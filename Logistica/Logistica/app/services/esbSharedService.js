define(["app"], function(app) {
    "use strict";
    app.factory('esbSharedService', function($rootScope, localStorageService) {
        var sharedService = {};
        sharedService.callbacks = {};
        sharedService.user = null;

        sharedService.isSignalRConnection = false;


        sharedService.prepForBroadcast = function(msg) {
            sharedService.message = msg;
            this.broadcastItem();
        };

        sharedService.broadcastItem = function() {
            $rootScope.$broadcast('handleBroadcast');
        };


         sharedService.ProcessUser = function(user) {

            user.uiImage = "";
            if (user.ImgUrl != "" && user.ImgUrl != undefined) {
                user.uiImage = "Images\\Avatar\\" + user.ImgUrl;
            } else {
                user.uiImage = "Images\\no-image.png";
            }
            user.name = user.FirstName;
            if (user.name == "" || user.name == null || user.name == undefined) {
                user.name = user.Email;
            }
        }

        sharedService.loggedIn = function(user, remember) {
            sharedService.ProcessUser(user);
            sharedService.user = user;
            if (remember) {
                localStorageService.set('lu', user);
            }
            $rootScope.$broadcast('onUserLogged');
        };

        sharedService.updateLocalStorage = function() {
            var lu = localStorageService.get('lu');
            if (lu == undefined || lu == "") {
                return;
            }
            localStorageService.set('lu', sharedService.user);

        };

        sharedService.logOut = function() {
            sharedService.user = null;
            localStorageService.set('lu', null);
            $rootScope.$broadcast('onUserLogged');
        };

       

        this.GetLoggedUser = function() {
            var lu = localStorageService.get('lu');
            if (lu == undefined || lu == "") {
                sharedService.user = null;
            } else {
                sharedService.ProcessUser(lu);
                sharedService.user = lu;
                sharedService.loggedIn(lu, false);
            }
        }
        this.GetLoggedUser();


        sharedService.localizationReceived = function(localization) {
            sharedService.localization = localization;
            $rootScope.$broadcast('onLocalizationReceived');
        };

        sharedService.fileChanged = function(changedFile) {
            sharedService.changedFile = changedFile;
            $rootScope.$broadcast('onChangedFile');
        };

        sharedService.signalRConnectionEstablished = function(isAlive) {
            sharedService.isSignalRConnection = isAlive;
            $rootScope.$broadcast('onSignalRConnectionEstablished');
        };

        sharedService.enablePermissions = function(isEnabled) {
            sharedService.isPermissionsEnabled = isEnabled;
            $rootScope.$broadcast('onShowHidePermissions');
        };

        sharedService.mapValuesChanged = function (isEnabled) {
            $rootScope.$broadcast('onMapValuesChanged');
        };

        sharedService.executeFunction = function(it) {
            sharedService.functionName = it.controller + "." + it.action;
            var mainScope = angular.element("#esbBody").scope();

            if (mainScope.ViewPath != it.viewPath) {
                mainScope.ViewPath = it.viewPath;
            }
        };

        return sharedService;
    });
});