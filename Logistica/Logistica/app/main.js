/// <reference path="controllers/main/MainController.js" />
/// <reference path="controllers/AppController.js" />
/// <reference path="services/esbSharedService.js" />
require.config({
    baseUrl: 'app',
    urlArgs: 'v=1.0',
    paths: {
        // the left side is the module ID,
        // the right side is the path to
        // the jQuery file, relative to baseUrl.
        // Also, the path should NOT include
        // the '.js' file extension. This example
        // is using jQuery 1.9.0 located at
        // js/lib/jquery-1.9.0.js, relative to
        // the HTML page.
        
    }
});
	
 //'acemaster/ace/ace',

require(
    [
       
        'app',
        'mapApp',
        'services/routeResolver',
        'services/config',
        'services/httpInterceptors',
        'services/modalService',


        'services/esbSharedService',
        'services/notificationService',
        'services/utils',
        'services/pusherService',
        'services/securityService',
        
        'controllers/AppController',
        'controllers/menu/MenuController',

        'directives/map_controller',
        'directives/map',
        'directives/marker',
        'directives/shape',
         'directives/directions',
         'directives/location-picker',


        'services/geo_coder',
         'services/navigator_geolocation',
          'services/attr2_options',

          'services/location',
          'services/reverse-geocoder',


    ],
    function () {
        angular.bootstrap(document, ['customersApp']);
    });
