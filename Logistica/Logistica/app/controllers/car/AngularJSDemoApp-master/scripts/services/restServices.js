(function () {
    'use strict';
    var demoApp = angular.module('restModule',['ngResource' // REST api calls
                                              ]);
    demoApp.value('restApiConfig', {
        countries:{ url: '/json/countries.json'}
    });
 
    demoApp.factory('RestService', ['$resource', 'restApiConfig', 
        function ($resource, restApiConfig) {
            //compile the rest service URL from the configuration file /common/config.js
            var factory = function() {};

            factory.prototype.getCountries = function () {
                return $resource(restApiConfig.countries.url, {}, {});
            };

            var service = {
                getInstance: function () { return new factory(); }
            };

            return service;
        }]);
}());
