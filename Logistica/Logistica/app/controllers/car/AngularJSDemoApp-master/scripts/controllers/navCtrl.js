(function () {
    'use strict';
    var demoModule = angular.module('demoApp');
    
    demoModule.controller('navCtrl', navCtrlMethod);

    function navCtrlMethod($scope) {
        $scope.applicationName = "AngularJS Demo Application";
    };
}());