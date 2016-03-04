define(["app"], function(app) {
    "use strict";
    app.service('pusherService', function($q, esbSharedService, securityService, utils) {



        this.channelId = utils.uuid();



        var SignalRManager = function() {
            var srServer = null;
            var callbacks = {};
            var lastRequest = null;

            if ($.connection == null)
                return;

            //jQuery.support.cors = true;
            //$.connection.hub.logging = true;
            //$.connection.hub.url = "http://localhost:8090/signalr";
            srServer = $.connection.sRLogisticServer;


            $.connection.hub.disconnected(function() {
                //esbSharedService.signalRConnectionEstablished(false);
            });

            srServer.client.onResponse = function(msg) {
                console.log("Receive message");
                if (msg == undefined || msg == null)
                    return;

                var receivedObj = JSON.parse(msg);
                var jsonResponse = null;
                var error = null;
                if (receivedObj.Error == null) {
                    jsonResponse = JSON.parse(receivedObj.JsonResponse);
                } else {
                    error = receivedObj.Error;
                }
                if (callbacks[receivedObj.RequestGuid] != undefined) {
                    var aaa = Object.keys(callbacks).length;

                    console.log("c length = " + aaa);
                    var ang = callbacks[receivedObj.RequestGuid];
                    if (ang != undefined) {
                        ang.scope.$apply(function() {
                            ang.promise(jsonResponse, error);
                            delete callbacks[receivedObj.RequestGuid];
                        });
                    }

                }
            }

            srServer.client.onMsgConfirm = function(msg) {
                console.log("Receive key " + msg);
                securityService.securityToken = msg;
            };

            srServer.client.onSecurityError = function(msg) {
                console.log("Security Error " + msg);
            };

            this.StartConnection = function() {
                $.connection.hub.start()
                    .done(function() {
                        console.log("connected...");
                        esbSharedService.signalRConnectionEstablished(true);
                        if(lastRequest != null)
                        {
                            this.SendMessage(lastRequest.msg,lastRequest.promise,lastRequest.scope);                            
                        }
                    })
                    .fail(function(E) {
                        console.log('Could not connect');
                    });
            };

            this.SendMessage = function(request, promise, scope) {
                if ($.signalR.connectionState.connected != $.connection.hub.state) {
                    if (promise != undefined) {
                        lastRequest =  {
                            msg:request,
                            promise: promise,
                            scope: scope
                        };
                    }
                    this.StartConnection();
                    return;
                }
                if (promise != undefined) {
                    callbacks[request.requestGuid] = {
                        promise: promise,
                        scope: scope
                    };
                }
                var aaa = Object.keys(callbacks).length;
                console.log("callbacks length = " + aaa);

                var jsonString = JSON.stringify(request);
                srServer.server.requestFromClient(jsonString).fail(function(e) {
                    console.log("send SR message exception " + e);
                });
            }

            this.StartConnection();
        };


        var signalRManager = new SignalRManager();

        this.PushESBMessage = function(msg, promise, scope) {

            this.PushESBMessageWithSignalR(msg, promise, scope);
        };

        this.PushESBMessageWithSignalR = function(msg, promise, scope) {
            msg.accessToken = securityService.securityToken;
            msg.userName = "";

            if (securityService.LoggedUser != null) {
                msg.userName = securityService.LoggedUser.Name;
            }
            msg.requestGuid = utils.uuid();
            var jsonString = JSON.stringify(msg);

            signalRManager.SendMessage(msg, promise, scope);

            return false;
        };



    });
});