(function () {
    'use strict';
    var demoModule = angular.module('demoApp');
    
    demoModule.controller('controlsCtrl', controlsCtrlMethod);

    function controlsCtrlMethod($scope, $routeParams, demoService) {
	  	setTimeout(function () {
	  	//	$scope.dropDown = {id:1, name:'first item'};
	        $scope.$apply(function () {
	            $scope.dropDown = {id:2, name:'2nd item'};
	        });
    	}, 2000);

    	//set countries
    	demoService.getCountries().then(function(data){
    		$scope.countries = data.countries;
    	});

    	$scope.controlData = [{id:1, name:'first item'},{id:2, name:'2nd item'},{id:1, name:'3rd item'}];
    	$scope.changeDropDown =function(elm){
    		console.log(elm);
    	}
    };
}());