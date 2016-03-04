using ESB.Utils.Reflection;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESB.Utils.Serializers;
using Logistica.App_Start;

namespace Logistica.SRServer
{
    public class SRLogisticServer:Hub
    {
        public bool Ping()
        {
            Clients.Caller.onPing("ok");
            return true;
        }

        public bool RequestFromClient(string request)
        {
            Clients.Caller.onMsgConfirm("ok");
            LogisticMethod method = request.JsonDeserialize<LogisticMethod>();
            method.ChannelId = this.Context.ConnectionId;
            //method.RequestGuid = Guid.NewGuid();
            if (method.RKey == null)
            {
                method.RKey = "";
            }
            RMQConfig.Pusher.Send(method.ToJSON(),method.RKey);
            return true;
        }
        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }
    }
}
