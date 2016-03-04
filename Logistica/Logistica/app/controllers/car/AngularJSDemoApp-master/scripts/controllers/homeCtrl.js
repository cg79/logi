(function () {
    'use strict';
    var demoModule = angular.module('demoApp');
    
    demoModule.controller('homeCtrl', homeCtrlMethod);

    function homeCtrlMethod() {
    	this.info = "How do you do ?";
    	this.text = "test";
    };
}());