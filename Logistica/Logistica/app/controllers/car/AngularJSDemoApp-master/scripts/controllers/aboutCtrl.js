(function () {
    'use strict';
    var demoModule = angular.module('demoApp');
    
    demoModule.controller('aboutCtrl', aboutCtrlMethod);

    function aboutCtrlMethod($scope) {
    	$scope.pageData = {};
    	$scope.pageData.goods = [{price:10, name:'Chocolate'},{price:2, name:'Candy'},{price:5, name:'Juice'}];
        $scope.pageData['inputTextModel'] = "Test";
    	
    };
}());