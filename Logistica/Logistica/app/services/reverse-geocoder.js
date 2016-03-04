angular.module('ngMap')
  .factory('reverseGeocoder', [
    '$document',
    '$q',
    function($document, $q) {

      var service = {};

      $document.ready(function() {
        service.geocoder = new google.maps.Geocoder();
      });

      service.geocode = function(location) {
        var deferred = $q.defer();
        var latlng = null;
        if (!location) {

          deferred.reject('You need to provide LatLng');

        } else {

           latlng = new google.maps.LatLng(location.latitude, location.longitude);
        }


        // geocode
        service.geocoder.geocode({
          latLng: latlng
        }, function(results, status) {
          if (status !== google.maps.GeocoderStatus.OK) {

            deferred.reject('No locations found');
            return;

          } else {
            var resp = {
              geometry:
              {
                location:latlng
              },
              address_components:results[0].address_components
            };
            deferred.resolve(resp);
          }
        });

        return deferred.promise;
      };

      return service;
    }
  ]);