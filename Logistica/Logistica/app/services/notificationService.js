define(["app"], function (app) {
    "use strict";
    app.service('notificationService', function (esbSharedService) {
        //http://codeseven.github.io/toastr/demo.html
        toastr.options = {
            "closeButton": true,
            "progressBar": true
        }
        this.ShowLocalizationMessage = function (title, code, success, options) {
            var msg = esbSharedService.localization[code];
            this.ShowMessage(title, msg, success, options);
        };

        this.ShowMessage = function (title, msg, success, options) {
            if (options != undefined) {
                toastr.options = options;
            }
            if (success == undefined) {
                toastr.success(msg, title);
                return;
            }
            switch (success) {
                case 0://success
                    {
                        toastr.success(msg, title);
                        break;
                    }
                case 1://error
                    {
                        toastr.error(msg, title);
                        break;
                    }
                case 2://warning
                    {
                        toastr.warning(msg, title);
                        break;
                    }
                case 3://info
                    {
                        toastr.info(msg, title);
                        break;
                    }

            }

        };

        this.ShowSuccessMessage = function (msg) {
            toastr.success(message, "User");
        };
        this.ShowWarningMessage = function (msg) {
            toastr.success(message, "User");
        };
    });
});