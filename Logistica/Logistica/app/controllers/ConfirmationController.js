define(['app'], function (app) {
    'use strict';
    app.register.controller('ConfirmationController',
        ['$scope', '$modalInstance', 'inputParameter', function ($scope, $modalInstance, inputParameter) {

            $scope.SelectedOption = false;
            $scope.message = "";
            if (inputParameter != undefined && inputParameter.message != undefined) {
                $scope.message = inputParameter.message;
            }
            $scope.ok = function () {
                $scope.SelectedOption = true;
                $modalInstance.close($scope.SelectedOption);
            };

            $scope.cancel = function () {
                $modalInstance.dismiss('cancel');
            };
        }]);
});
