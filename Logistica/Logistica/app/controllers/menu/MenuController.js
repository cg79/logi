define(['app'], function (app) {
    'use strict';

    app.controller('MenuController', ['$scope', '$modal', '$location', 'securityService', 'pusherService', 'esbSharedService', 'utils', function ($scope, $modal, $location,securityService, pusherService, esbSharedService, utils) {
    
        $scope.ControllerName = "MenuController";
    $scope.menuItems = [];
    $scope.isVisible = false;
    
    $scope.x = 0;
    $scope.y = 0;
    $scope.fNameVar = securityService.functionalityName;
    $scope.permissions = securityService.permissions;

    $scope.LoggedUser = securityService.LoggedUser;

    $scope.$on('onPermissionsChanged', function () {
        $scope.$apply(function () {
            $scope.permissions = securityService.permissions;
            if (securityService.LoggedUser != null && securityService.LoggedUser != undefined ) {
                $scope.ShowUserControls = securityService.LoggedUser.ID > 0;
            }
        });
    });
   

    $scope.showPermissionsPopup = function (id, text, event) {
        if (event != undefined) {
            event.preventDefault();
            event.stopPropagation();
        }
        var mydata = {
            id:id,
            featureName: text,
        };
        require(
                       [
                           "controllers/SecurityPermissionsController"
                       ],
                        function () {
                            var modalInstance = $modal.open({
                                templateUrl: 'app/views/security/permissionRoles.html?id=1',
                                controller: 'SecurityPermissionsController',
                                resolve: {
                                    inputParameter: function () {
                                        return mydata;
                                    }
                                }
                            });
                        });
    };
    $scope.createMenuItem = function (id,text,controller, action, cssClass,viewPath)
    {
        var menu =
            {
                id:id,
                text: text,
                controller:controller,
                action: action,
                cssClass: cssClass,
                viewPath:viewPath
            };
        return menu;
    };

    $scope.menuItems.push(
        $scope.createMenuItem(
            "03ad6367-ce33-4c44-b4e1-eebaa82eed39",
            "Manage users",
            "SecurityController",
            "loadSecurityUsers",
            "fa fa-pencil",
            "/security/users"));

    $scope.menuItems.push(
        $scope.createMenuItem(
        "3b2b8898-6d65-429a-9988-0409de84c06a",
            "Transport",
            "SecurityController",
         "loadGroups",
         "fa fa-pencil",
         "/transport/search"));

     $scope.menuItems.push(
        $scope.createMenuItem(
        "3b2b8898-6d65-429a-9988-0409de84c06a",
            "Transport ADD",
            "SecurityController",
         "loadGroups",
         "fa fa-pencil",
         "/transport/add"));

 $scope.menuItems.push(
        $scope.createMenuItem(
        "3b2b8898-6d65-429a-9988-0409de84c06a",
            "New User",
            "SecurityController",
         "loadGroups",
         "fa fa-pencil",
         "/user/create"));

   
    $scope.menuItems.push(
        $scope.createMenuItem(
        "227f6acb-afa4-43fa-bebc-ec67af9f92de",
            "Manage roles",
            "SecurityController",
          "loadRoles",
          "fa fa-pencil",
          "/security/roles"));

    $scope.menuItems.push(
       $scope.createMenuItem(
        "e9aa2193-6539-4fe4-a778-33fb21054658",
           "User-Group matrix",
           "SecurityUsersGroupsController",
           "loadUsersGroups",
           "fa fa-pencil",
           "/security/user/group"));

    

    $scope.menuItems.push(
        $scope.createMenuItem(
        "931225e8-d51b-4460-b5dd-43517ee45b68",
         "User-Roles matrix",
         "SecurityUsersRolesController",
         "loadUsersRoles",
         "fa fa-pencil",
         "/security/user/role"));

    $scope.menuItems.push(
       $scope.createMenuItem(
        "8b1b85f7-5a70-49ea-8924-e244693cc7eb",
        "Groups-Roles matrix",
        "SecurityGroupsRolesController",
        "loadGroupsRoles",
        "fa fa-pencil",
        "/security/group/role"));

    $scope.auditItems = [];

    $scope.auditItems.push(
       $scope.createMenuItem(
        "11b27267-0db6-40e4-b3c7-fa8a9f72785b",
        "Items",
        "AuditController",
        "loadAudits",
        "fa fa-pencil",
        "/audit"));

    $scope.errorItems = [];

    $scope.errorItems.push(
       $scope.createMenuItem(
        "1cdf3675-ea63-4201-8587-d11c5600fdf1",
        "Items",
        "ErrorController",
        "loadItems",
        "fa fa-pencil",
        "/errors"));

    $scope.advWorksItems = [];

    $scope.advWorksItems.push(
       $scope.createMenuItem(
        "d69bf82f-6bd0-4144-8cdd-dbebb4951d2f",
        "Adventure Works Items",
        "SFPUserController",
        "loadSFPUser",
        "fa fa-pencil",
        "/adventureworks"));

    $scope.menuItemClicked = function (it)
    {
        $location.url(it.viewPath);
        return;
        var item = utils.cloneObject(it);
        item.viewPath = item.viewPath;// + "?id='" + utils.uuid()+"'";
        esbSharedService.executeFunction(item);
    }
    $scope.isPermissionsEnabled = esbSharedService.isPermissionsEnabled;
    $scope.$on('onShowHidePermissions', function () {
        $scope.isPermissionsEnabled = esbSharedService.isPermissionsEnabled;
    });
  

    }]);


});