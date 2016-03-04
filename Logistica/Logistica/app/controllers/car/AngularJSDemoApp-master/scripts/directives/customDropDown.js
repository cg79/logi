(function(){
'use strict';
angular.module('demoApp').directive('customDropdown', function() {
  return {
    restrict: 'E',
    replace: true,
    transclude:true,
    scope: {
      elements:'=',
      selectModel:'=',
      onChangeMethod:'&',
      label:"@"
    },
    templateUrl: '/views/custom-dropdown.html',
    controller: function ($scope, $element, $attrs) {
      
      $scope.applyElementSelection = function(element){
        $scope.selectModel = element;
        $scope.onChangeMethod({ element:element });
      }
    }
  }
});
}());
