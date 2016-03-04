using EasyNetQ.DTO;
using Logistica.SRServer;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using ESB.Utils.Serializers;

namespace Logistica.App_Start
{
    public class RMQConfig : IRegisteredObject
    {
        private IHubContext hubContext = null;
        private RabbitMQPublisher rabbitMQPublisher = null; 

        private RabbitMQResponseConsumer rabbitMQResponseConsumer=null;
        public Action<Response> actionResponse = null;

        public static RabbitMQPublisher Pusher = null; 
        public RMQConfig()
        {
            HostingEnvironment.RegisterObject(this);

            #region Request
            rabbitMQPublisher = new RabbitMQPublisher();
            rabbitMQPublisher.SetQueueProperties(new QueuePropertiesPublisher()
            {
                AutoDelete = false,
                Durable = true,
                ExchangeProperties = new ExchangeProperties() { ExchangeName = "LResponse", ExchangeType = "fanout" },
                RoutingKey = string.Empty
            });
            rabbitMQPublisher.Start();
            #endregion

            #region Request
            Pusher = new RabbitMQPublisher();
            Pusher.SetQueueProperties(new QueuePropertiesPublisher()
            {
                AutoDelete = false,
                Durable = true,
                ExchangeProperties = new ExchangeProperties() { ExchangeName = "LPusher", ExchangeType = "fanout" },
                RoutingKey = string.Empty
            });
            Pusher.Start();
            #endregion

            #region Response
            actionResponse = new Action<Response>(a => receiveResponse(a));



            rabbitMQResponseConsumer = new RabbitMQResponseConsumer();
            rabbitMQResponseConsumer.SetQueueProperties(new QueuePropertiesReceiver()
            {
                AutoDelete = false,
                Durable = true,
                ExchangeProperties = new ExchangeProperties() { ExchangeName = "LResponse", ExchangeType = "fanout" },
                QueueName = "QLResponse",
                RoutingKey = string.Empty
            });

            rabbitMQResponseConsumer.SetMethodForReceivedMessage(actionResponse);

            rabbitMQResponseConsumer.Start();
            #endregion


            hubContext = GlobalHost.ConnectionManager.GetHubContext<SRLogisticServer>();
        }

        void receiveResponse(Response obj)
        {
            hubContext.Clients.Client(obj.ChannelId).onResponse(obj.ToJSON());
        }


        public void Stop(bool immediate)
        {
            if (rabbitMQPublisher == null)
                return;
            rabbitMQPublisher.Stop();
            rabbitMQPublisher = null;
            

            if (rabbitMQResponseConsumer == null)
                return;
            rabbitMQResponseConsumer.Stop();
            rabbitMQResponseConsumer = null;

            HostingEnvironment.UnregisterObject(this);
        }
    }
}
