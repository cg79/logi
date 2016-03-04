define(["app"], function(app) {
    "use strict";
    app.service('imageService', function($q, $timeout) {


        var fileReaderSupported = window.FileReader != null && (window.FileAPI == null || FileAPI.html5 != false);
        var image = null;
        var imageUrl = null;


        this.showOpenFileDlg = function(id) {
            if (id != undefined) {
                angular.element("#" + id).click();
            } else {
                angular.element("#fileupload").click();
            }
        };

        this.generateThumb = function(file) {
            if (file == null || file == undefined)
                return;

            if (fileReaderSupported && file.type.indexOf('image') > -1) {
                $timeout(function() {
                    var fileReader = new FileReader();
                    fileReader.readAsDataURL(file);
                    fileReader.onload = function(e) {
                        $timeout(function() {
                            file.dataUrl = e.target.result;
                            image = file;
                            imageUrl = file.dataUrl;
                        });
                    }
                });
            }
        };



    });
});