define(['app'], function (app) {
    'use strict';
    app.register.controller('ResetPasswordController',
        ['$scope', '$modal', '$location','$timeout','$upload', 'securityService', 'pusherService', 'esbSharedService', 'utils', 'notificationService',
            function ($scope, $modal, $location, $timeout,$upload,securityService, pusherService, esbSharedService, utils, notificationService) {

        var inputObj = function()
        {
            this.EmailCode = "";
            this.Password = "";
            this.CPassword = "";
            this.ServerMessage = "";
            this.same = false;
        }        
        
        $scope.input = new inputObj();
        
       
       $scope.ui = null;
        var jqxhr = $.getJSON("app/localization/en/user.html", function (resp) {
            $scope.$apply(function () {
                                $scope.ui =  resp;
                           });

        })
          .fail(function (xxx) {
              console.log("error loading car ui");
        });

    
//     $scope.$watch('input.Password', function(newv,oldv) {
//         if(newv == null || newv==undefined||newv=="")
//             return;
//         if($scope.input.Password == $scope.input.CPassword)
//         {
//             $scope.input.same = true;

//         }else{
//             $scope.input.same = false;    
//         }
//     });
// $scope.$watch('input.CPassword', function(newv,oldv) {
//         if(newv == null || newv==undefined||newv=="")
//             return;
//         if($scope.input.Password == $scope.input.CPassword)
//         {
//             $scope.input.same = true;

//         }else{
//             $scope.input.same = false;    
//         }
//     });
      $scope.resetPassword = function () {
        if (this.frm.$invalid) {
                //alert("invalid");
                return;
            }
            if($scope.input.Password != $scope.input.CPassword)
            {
                //this.frm.password.$setValidity("required", true);
                this.frm.password.$setValidity("notmatch", false);
                return;
            }
                var mmm =
            {
                Library:"UsersImplementation",
                Namespace:"UsersImplementation.Repositories.UsersRepository",
                Method:"UIResetPassword",
                JSON:"" 
            };
            mmm.JSON = JSON.stringify($scope.input);

            pusherService.PushESBMessage(mmm,$scope.OnResetPassword, $scope);

            };


    $scope.OnResetPassword = function (obj, error) {
                //$scope.loadSecurityUsers();

                if (obj.R == null || obj.R == '') {
                   // $scope.$apply(function () {
                                 $scope.input.ServerMessage = $scope.ui.USER_SAVED;
                           //});
                   
                } else {
                    //$scope.$apply(function () {
                               console.log(obj.R);
                    $scope.input.ServerMessage =  $scope.ui[obj.R];
                           //});
                    
                }
            };







        }]);

});

    


    