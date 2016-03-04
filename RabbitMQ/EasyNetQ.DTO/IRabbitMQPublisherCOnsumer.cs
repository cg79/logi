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
    public abstract class IRabbitMQPublisherConsumer
    {
        
        public abstract void Start();
        public abstract void Stop();

        public abstract void Send(string message);
        
    }
}
