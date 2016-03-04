using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ESB.Utils.Serializers;

namespace EasyNetQ.DTO
{
    public abstract class IRabbitMQConsumer<T>
    {
        public event Action<string> ResponseReceivedEvent;
        protected Action<string> methodExecutedWhenAMessageIsReadfromQueue;
        protected QueuePropertiesReceiver queueProp;
        protected IConnection connection = null;
        protected IModel channelReceive = null;
        protected Thread receivingThread = null;
       
        
        public abstract void SetQueueProperties(QueuePropertiesReceiver queueProp);
        public abstract void Start();
        public abstract void Stop();


        public void SetMethodForReceivedMessage(Action<string> method)
        {
            this.methodExecutedWhenAMessageIsReadfromQueue = method;
        }

        protected void MessageReceived(IModel channel, DefaultBasicConsumer consumer, BasicDeliverEventArgs eventArgs)
        {
            string message = Encoding.UTF8.GetString(eventArgs.Body);
            //T request = message.JsonDeserialize<T>();

            if (ResponseReceivedEvent != null)
            {
                ResponseReceivedEvent(message);
                return;
            }

            methodExecutedWhenAMessageIsReadfromQueue(message);
            //if (MessageReceivedEvent != null)
            //{
            //    MessageReceivedEvent(message);
            //}
           
        }
    }
}
