(function (){
	'use strict';

/*
 * Main module of the application.
 */
angular.module('demoApp', [
  	'ngAnimate',    // animations
    'ngRoute',      // routing with ng route [routes]
    'ui.router',    // routing with ui router [states]
    'ngSanitize',   // sanitizes html bindings 
    'restModule'    //rest services module
  ])
.config(function ($routeProvider, $stateProvider, $httpProvider, $locationProvider) {
    $routeProvider
    .when('/demo', {
      templateUrl: '/views/demo.html',
      controller: 'demoCtrl'
    })
    .when('/demo/:firstParam/:secondParam', {
      templateUrl: '/views/demo.html',
      controller: 'demoCtrl'
    })
    .when('/login', {
      templateUrl: '/views/login.html',
      controller: 'loginCtrl'
    });
/*  .otherwise({
    redirectTo: '/login'
  });*/

    //routing based on states
    $stateProvider 
    .state('demo', {
      url: '/demo',
      templateUrl: '/views/_layout.html',
      controller: 'homeCtrl'
    })
    .state('demo.container', {
      url: '/container',
      template: 'Container text'
    })
    .state('demo.login', {
      url: '/loginContainer',
      templateUrl: '/views/login.html',
      controller: 'demoCtrl'
    })
    .state('home', {
      url: '/home',
      templateUrl: '/views/home.html',
      controller: 'homeCtrl',
      controllerAs: 'sc'
    })
    .state('home.list', {
        url: '/list',
        templateUrl: 'views/partial-home.html',
        controller: function($scope) {
            $scope.dogs = ['Bernese', 'Husky', 'Goldendoodle'];
        }
    })
    .state('home.paragraph', {
        url: '/paragraph',
        template: 'I am a paragraph'
    })
    .state('about', {
        url: '/about',
        views: {
            // the main template will be placed here (relatively named)
            '': { templateUrl: '/views/about.html' },
            // the child views will be defined here (absolutely named)
            'columnOne@about': { template: 'Look I am a column!' },
            // for column two, we'll define a separate controller 
            'columnTwo@about': { 
                templateUrl: '/views/table-data.html',
                controller: 'aboutCtrl'
            }
        }
    })
    .state('demoParams', {
      url: '/demo/:firstParam/:secondParam',
      templateUrl: '/views/demo.html',
      controller: 'demoCtrl'
    })
    .state('login', {
      url: '/login',
      templateUrl: '/views/login.html',
      controller: 'loginCtrl'
    })
    .state('controls',{
      url:'/controls',
      templateUrl:'/views/controls.html',
      controller:'controlsCtrl'
    });

   //$locationProvider.html5Mode(true);
   $httpProvider.interceptors.push('httpResponseInterceptor');
  })
.factory('httpResponseInterceptor',['$q','$location','$window',function($q,$location,$window){
      return {
        'request': function (request) {
          if (request.url.indexOf('controls') != -1) {
              var date = new Date();
              request.url = request.url.replace(/[?|&]cacheBuster=\d+/, '');
              request.url += request.url.indexOf('?') === -1 ? '?' : '&';
              request.url += 'cacheBuster=' + date.getTime();
          }

              return request || $q.when(request);
        },
        response: function(response){
          if (response.status === 401) {
            console.log("Response 401");
          }
          return response || $q.when(response);
        },
        responseError: function(rejection) {
          if (rejection.status === 401) {
            console.log("Response Error 401",rejection);
            $location.path('/login').search('returnTo', $location.path());
            //$window.location.reload();
          }
          return $q.reject(rejection);
        }
      }
}])
.run(['$rootScope', '$window', '$location', '$http',
    function ($rootScope, $window, $location, $http) {

      $rootScope.$on('$routeChangeStart', function (event, next, current) {
        $rootScope.user = {};
        $rootScope.user.isAuthenticated = true;
        if ($rootScope.user && $rootScope.user.isAuthenticated) {
          $http.defaults.headers.common['Authorization'] = 'Basic authentication data';
        }
        // redirect to login page if not logged in
        if ($location.path() !== '/login' && !$rootScope.user) {
            $location.path('/login');
        }
      });
}]);
}())