using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyNetQ.DTO
{
    public static class ChannelExtensions
    {
        // http://lostechies.com/derekgreer/2012/05/29/rabbitmq-for-windows-headers-exchanges/
        public static void StartConsume(this IModel channel, string queueName, Action<IModel, DefaultBasicConsumer, BasicDeliverEventArgs> callback)
        {
            QueueingBasicConsumer consumer = new QueueingBasicConsumer(channel);
            channel.BasicConsume(queueName, true, consumer);

            while (true)
            {
                try
                {
                    var eventArgs = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                    callback(channel, consumer, eventArgs);
                }
                catch (EndOfStreamException)
                {
                    // The consumer was cancelled, the model closed, or the connection went away.
                    break;
                }
            }
        }
    }
}
