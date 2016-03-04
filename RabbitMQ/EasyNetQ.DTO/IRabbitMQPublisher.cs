using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EasyNetQ.DTO
{
    public abstract class IRabbitMQPublisher
    {
        protected QueuePropertiesPublisher queueProp;
        protected IConnection connection = null;
        protected IModel channelSend = null;


        public abstract void SetQueueProperties(QueuePropertiesPublisher queueProp);
        public abstract void Start();
        public abstract void Stop();
        //public abstract void Send(string input);
        public abstract void Send(string input,string routingKey);

       
    }
}
