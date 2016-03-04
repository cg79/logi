define(['app'], function (app) {
    'use strict';
    app.register.controller('MainController',
        ['$scope',  '$location', 'pusherService', 'esbSharedService', 'utils',
            function ($scope, $location, pusherService, esbSharedService, utils) {

        
      $scope.user =  esbSharedService.user;










        }]);

});

    


    