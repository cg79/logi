using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ESB.Utils.Reflection;
using ESB.Utils.Serializers;

namespace EasyNetQ.DTO
{
    public class RabbitMQPublisherConsumer : IRabbitMQPublisherConsumer,IDisposable
    {

        
        protected QueuePropertiesPublisher queuePropSender;
        private RabbitMQPublisher rabbitMQSender;

        protected QueuePropertiesReceiver queuePropReceiver;
        private RabbitMQRequestConsumer rabbitMQReceiver;

        public RabbitMQPublisherConsumer SetQueuePropertiesForSender(QueuePropertiesPublisher queuePropSender)
        {
            this.queuePropSender = queuePropSender;
            return this;
        }

        public RabbitMQPublisherConsumer SetQueuePropertiesForReceiver(QueuePropertiesReceiver queuePropReceiver)
        {
            this.queuePropReceiver = queuePropReceiver;
            return this;
        }
        protected Action<string> methodExecutedWhenAMessageisReadfromQueue;
        public RabbitMQPublisherConsumer SetMethodForReceivedMessage(Action<string> method)
        {
            this.methodExecutedWhenAMessageisReadfromQueue = method;
            return this;
        }

       
        public void Dispose()
        {
            Stop();
        }

        public override void Start()
        {
            if (queuePropSender.ExchangeProperties.ExchangeName == queuePropReceiver.ExchangeProperties.ExchangeName)
            {
                throw new Exception("queuePropSender.ExchangeProperties.ExchangeName == queuePropReceiver.ExchangeProperties.ExchangeName " + queuePropSender.ExchangeProperties.ExchangeName );
            }
            rabbitMQSender = new RabbitMQPublisher();
            rabbitMQSender.SetQueueProperties(queuePropSender);
            rabbitMQSender.Start();

            rabbitMQReceiver = new RabbitMQRequestConsumer();
            rabbitMQReceiver.SetQueueProperties(queuePropReceiver);
            if (methodExecutedWhenAMessageisReadfromQueue == null)
            {
                methodExecutedWhenAMessageisReadfromQueue = new Action<string>(lm =>
                    {

                        //Response obj = lm.ExecuteMethod();
                        //obj.ChannelId = lm.ChannelId;
                        //obj.RequestGuid = lm.RequestGuid;
                        //return obj;

                        rabbitMQSender.Send(lm, queuePropSender.RoutingKey);
                    });
            }
            rabbitMQReceiver.SetMethodForReceivedMessage(methodExecutedWhenAMessageisReadfromQueue);

            //rabbitMQReceiver.MessageReceivedEvent += rabbitMQReceiver_MessageReceivedEvent;
            rabbitMQReceiver.Start();
        }

       
        //void rabbitMQReceiver_MessageReceivedEvent(string obj)
        //{
        //    //kl;
        //    rabbitMQSender.Send(obj);
        //}

        public override void Stop()
        {
            if (rabbitMQSender != null)
            {
                rabbitMQSender.Stop();
                rabbitMQSender = null;
            }

            if (rabbitMQReceiver != null)
            {
                //rabbitMQReceiver.MessageReceivedEvent -= rabbitMQReceiver_MessageReceivedEvent;
                rabbitMQReceiver.Stop();
                rabbitMQReceiver = null;
            }
        }

        public override void Send(string message)
        {
            if (rabbitMQSender == null)
            {
                throw new Exception("Sender object is null");
            }
            rabbitMQSender.Send(message, queuePropSender.RoutingKey);

        }
    }
}
