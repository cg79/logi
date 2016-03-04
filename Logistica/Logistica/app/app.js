'use strict';

define(['services/routeResolver'], function () {

    var app = angular.module('customersApp',
        [
            'ngRoute',
            'ngResource',
            'routeResolverServices',
            'ui.bootstrap',
            'ui.utils',
            'smart-table',

            'LocalStorageModule',
            'ngMap',
            'ngSanitize',
            'ui.select',
            'angularFileUpload'
    ]);

    app.config(['$routeProvider', 'routeResolverProvider', '$controllerProvider',
                '$compileProvider', '$filterProvider', '$provide', '$httpProvider',

        function ($routeProvider, routeResolverProvider, $controllerProvider,
                  $compileProvider, $filterProvider, $provide, $httpProvider) {

            //Change default views and controllers directory using the following:
            //routeResolverProvider.routeConfig.setBaseDirectories('/app/views', '/app/controllers');

            app.register =
            {
                controller: $controllerProvider.register,
                directive: $compileProvider.directive,
                filter: $filterProvider.register,
                factory: $provide.factory,
                service: $provide.service
            };

            //Define routes - controllers will be loaded dynamically
            var route = routeResolverProvider.route;

            $routeProvider
                //route.resolve() now accepts the convention to use (name of controller & view) as well as the 
                //path where the controller or view lives in the controllers or views folder if it's in a sub folder. 
                //For example, the controllers for customers live in controllers/customers and the views are in views/customers.
                //The controllers for orders live in controllers/orders and the views are in views/orders
                //The second parameter allows for putting related controllers/views into subfolders to better organize large projects
                //Thanks to Ton Yeung for the idea and contribution

                //.when('/auth', route.resolve('Auth', 'auth/', 'vm'))
                 .when('/login', route.resolve('Login', 'login/', 'vm',false))
                 .when('/main', route.resolve('Main', 'main/', 'vm'))
                .when('/book', {
                    templateUrl: 'book.html',
                    controller: 'BookController',
                    resolve: {
                      // I will cause a 1 second delay
                      delay: function($q, $timeout) {
                        var delay = $q.defer();
                        $timeout(delay.resolve, 1000);
                        return delay.promise;
                      }
                    }})
                //.when('/security/usermessages', route.resolve('View_UserMessages', 'security/', 'vm'))
                .when('/security/users', route.resolve('Users', 'security/', 'vm'))
                .when('/security/email', route.resolve('Email', 'email/', 'vm', false))
                .when('/security/groups', route.resolve('Groups', 'security/', 'vm'))
                .when('/security/roles', route.resolve('Roles', 'security/', 'vm'))
                .when('/security/user/group', route.resolve('UserGroup', 'security/', 'vm'))
                .when('/security/user/role', route.resolve('UserRole', 'security/', 'vm'))
                .when('/security/group/role', route.resolve('GroupRole', 'security/', 'vm'))

                .when('/transport/search', route.resolve('TransportSearch', 'transport/', 'vm'))
                .when('/transport/add', route.resolve('TransportAdd', 'transport/', 'vm'))
                .when('/car/add_edit', route.resolve('CarAddEdit', 'car/', 'vm'))
                 .when('/car/list', route.resolve('CarList', 'car/', 'vm'))

                .when('/login', route.resolve('Login', 'login/', 'vm',false))

                .when('/user/create', route.resolve('CreateUser', 'user/', 'vm'))
     			.when('/user/login', route.resolve('Login', 'user/', 'vm',false))
                .when('/user/resetpassword', route.resolve('ResetPassword', 'user/', 'vm',false))
                .when('/user/forgotPassword', route.resolve('ForgotPassword', 'user/', 'vm',false))
                .when('/user/changePassword', route.resolve('ChangePassword', 'user/', 'vm',true))
                .when('/user/profil', route.resolve('Profil', 'user/', 'vm',false))

                .when('/company', route.resolve('Company', 'company/', 'vm',false))

                .when('/errors', route.resolve('Errors', 'errors/', 'vm'))

                .when('/audit', route.resolve('Audit', 'audit/', 'vm'))
                .when('/auditChanges', route.resolve('Changes', 'audit/', 'vm'))
                 .when('/password', route.resolve('Password', 'password/', 'vm', false))
                .when('/changepassword', route.resolve('ChangePassword', 'changepassword/', 'vm', true))

                .when('/adventureworks', route.resolve('AdventureWorks', 'adventureworks/', 'vm'))

                .when('/about', route.resolve('About', '', 'vm'))
                .when('/login/:redirect*?', route.resolve('Login', '', 'vm'))
                .otherwise({ redirectTo: '/main' });

    }]);

    app.run(['$rootScope', '$location', 
        function ($rootScope, $location) {
            
            //Client-side security. Server-side framework MUST add it's 
            //own security as well since client-based security is easily hacked
            $rootScope.$on("$routeChangeStart", function (event, next, current) {
                if (next && next.$$route && next.$$route.secure) {
                    //if (!authService.user.isAuthenticated) {
                    //    $rootScope.$evalAsync(function () {
                    //        authService.redirectToLogin();
                    //    });
                    //}
                }
            });

    }]);

    return app;

});





