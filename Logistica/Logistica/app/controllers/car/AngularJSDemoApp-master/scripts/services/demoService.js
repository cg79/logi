(function (){
'use strict';
var demoApp = angular.module('demoApp');
demoApp.factory('demoService', ['$http', '$q', '$rootScope', 'RestService', demoAppMethod]);  
	function demoAppMethod($http, $q, $rootScope, rest) {
	  var factory = {};
	  var restInstance = rest.getInstance();

	  factory.getCountries = function() {
	    return restInstance.getCountries().get().$promise;
	  };

	  factory.getCountriesHttp = function(){
      	var deferred = $q.defer();
      	$http.get('json/countries.json')
	        .success(function(data){
	          if (data){
	            deferred.resolve(data);
	          } else {
	            deferred.reject();
	          }
	        })
	        .error(function(){
	          deferred.reject();
	        });
      	return deferred.promise;
	  }

	  return factory;
	}

}());
 