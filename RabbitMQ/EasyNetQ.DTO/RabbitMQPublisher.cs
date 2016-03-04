using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EasyNetQ.DTO
{
    public class RabbitMQPublisher : IRabbitMQPublisher, IDisposable
    {

        public override void SetQueueProperties(QueuePropertiesPublisher queueProp)
        {
            this.queueProp = queueProp;
        }

        public override void Start()
        {
            try
            {
                var connectionFactory = new ConnectionFactory
                {
                    HostName = queueProp.ConnectionFactory.HostName,
                    Port = queueProp.ConnectionFactory.Port,
                    UserName = queueProp.ConnectionFactory.UserName,
                    Password = queueProp.ConnectionFactory.Password,
                    VirtualHost = queueProp.ConnectionFactory.VirtualHost
                };

                connection = connectionFactory.CreateConnection();

                channelSend = connection.CreateModel();

                channelSend.ExchangeDeclare(
                    queueProp.ExchangeProperties.ExchangeName,
                    queueProp.ExchangeProperties.ExchangeType,
                    queueProp.Durable,
                    queueProp.AutoDelete,
                    null);
            }
            catch (Exception ex)
            {
                throw new Exception("Most probably the exchange already exists and was previosly created with different properties than current excahnge type, durable or auto delete ",ex);
            }
        }

        public override void Stop()
        {
            if (channelSend != null && channelSend.IsOpen) channelSend.Close();
            if (connection != null && connection.IsOpen) connection.Close();
        }
        

        public void Dispose()
        {
            Stop();
        }


        //public override void Send(string input)
        //{
        //    byte[] message = Encoding.UTF8.GetBytes(input);
        //    channelSend.BasicPublish(
        //        queueProp.ExchangeProperties.ExchangeName,
        //        queueProp.RoutingKey,
        //        null, //IBasicProperties
        //        message);

        //}

        public override void Send(string input, string routingKey)
        {
            byte[] message = Encoding.UTF8.GetBytes(input);
            channelSend.BasicPublish(
                queueProp.ExchangeProperties.ExchangeName,
                routingKey,
                null, //IBasicProperties
                message);
        }
    }
}
