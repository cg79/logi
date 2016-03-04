define(["app"], function (app) {
    "use strict";
    app.service('securityService', function (localStorageService) {
        this.webroot = "";
        this.permissions = [];

        this.functionalityName = "xxx";
        this.appUser = function (id, name, firstName, lastName, email, guid, isGuest, avatar, useLocalStorage) {
            this.ID = id;
            this.Name = name;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.UserGuid = guid;
            this.IsGuest = isGuest;
            this.LoggedMode = "";
            this.Avatar = avatar;
            this.UseLocalStorage = useLocalStorage;
        };

        this.securityToken = "empty";
        this.encryptionKey = "";
        this.LoggedUser = null;

        this.GetLoggedUser = function () {
            var lg = localStorageService.get('appUser');
            if (lg == undefined || lg == "") {
                this.LoggedUser = new this.appUser(0, "Guest", null, null, null, null, true, "");
            } else {
                this.LoggedUser = new this.appUser(
                                            lg.ID,
                                            lg.Name,
                                            lg.FirstName,
                                            lg.LastName,
                                            lg.Email,
                                            lg.UserGuid,
                                            false,
                                            lg.Avatar,
                                            true
                    );
            }
        }

        this.GetLoggedUser();

        this.RememberUser = function (flag) {
            localStorageService.remove('appUser');
            if (flag == true) {
                localStorageService.set('appUser', this.LoggedUser);
            }
        }
        this.UpdateUserAvatar = function () {
            if (this.LoggedUser == null || this.LoggedUser == undefined || this.LoggedUser.UseLocalStorage == false)
                return;

            localStorageService.remove('appUser');
            localStorageService.set('appUser', this.LoggedUser);
        }

        this.SetLoggedUser = function (uID, uName, uFirstName, uLastName, uEmail, uUserGuid, uGuest, avatar) {
            ////localStorageService.remove('appUser');
            this.LoggedUser = new this.appUser(
                                            uID,
                                            uName,
                                            uFirstName,
                                            uLastName,
                                            uEmail,
                                            uUserGuid,
                                            uGuest,
                                            avatar);

            ////localStorageService.set('appUser', this.LoggedUser);

        }


        this.ResetLoggedUser = function () {
            localStorageService.remove('appUser');
            this.SetLoggedUser(0, "Guest");
        };
    });
});