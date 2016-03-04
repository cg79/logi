(function () {
    'use strict';
    var demoModule = angular.module('demoApp');
    
    demoModule.controller('loginCtrl', loginCtrlMethod);

    function loginCtrlMethod($scope) {
    	$scope.loginForm = { username: '', password: '' };
    	$scope.username = "";
    	$scope.password = "";
    	$scope.submitted = false;
    	$scope.login = function (isValid) {
    		$scope.submitted = true;
    	}

    };
}());