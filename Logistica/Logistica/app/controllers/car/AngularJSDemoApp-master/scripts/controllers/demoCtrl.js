(function () {
    'use strict';
    var demoModule = angular.module('demoApp');
    
    demoModule.controller('demoCtrl', demoCtrlMethod);

    function demoCtrlMethod($scope, $routeParams) {
    	this.variabila = 'test';
        //$scope.containerBkColor = 'background-color:red';
    };
}());