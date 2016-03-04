using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ESB.Utils.Serializers;

namespace EasyNetQ.DTO
{
    public class RabbitMQResponseConsumer : IRabbitMQConsumer<Response>, IDisposable
    {
        
        public override void SetQueueProperties(QueuePropertiesReceiver queueProp)
        {
            this.queueProp = queueProp;
        }
        
       
        public override void Start()
        {
            string log = "";
            try
            {
                //log = string.Concat(log, ""); 
                log = string.Concat(log, "Creating connection Factory " , queueProp.ToJSON(),Environment.NewLine); 
                var connectionFactory = new ConnectionFactory
                {
                    HostName = queueProp.ConnectionFactory.HostName,
                    Port = queueProp.ConnectionFactory.Port,
                    UserName = queueProp.ConnectionFactory.UserName,
                    Password = queueProp.ConnectionFactory.Password,
                    VirtualHost = queueProp.ConnectionFactory.VirtualHost
                };

                connection = connectionFactory.CreateConnection();

                log = string.Concat(log, "Subscribing to queue. Please ensure that the server to ", queueProp.ExchangeProperties.ExchangeName , " exchange is up and running ", Environment.NewLine); 

                channelReceive = connection.CreateModel();
                channelReceive.QueueDeclare(
                    queueProp.QueueName,
                    queueProp.Durable,
                    false,
                    queueProp.AutoDelete,
                    null);
                channelReceive.QueueBind(
                    queueProp.QueueName,
                    queueProp.ExchangeProperties.ExchangeName,
                    queueProp.RoutingKey,
                    null);

                log = string.Concat(log, "Creating listener thread ", Environment.NewLine);

                receivingThread = new Thread(() => channelReceive.StartConsume(queueProp.QueueName, MessageReceived));
                receivingThread.Name = string.Concat("ReceivingThread", queueProp.QueueName); //name the thread so that when it goes insane you will be able to apportion blame.
                receivingThread.Start();

                log = string.Concat(log, "All OK ", Environment.NewLine);
            }
            catch (Exception ex)
            {
                throw new Exception(log,ex);   
            }

        }

        public override void Stop()
        {
            if (channelReceive != null && channelReceive.IsOpen) channelReceive.Close();
            if (connection != null && connection.IsOpen) connection.Close();
            if (receivingThread != null) receivingThread = null;
        }

       

        public void Dispose()
        {
            Stop();
        }
    }
}
