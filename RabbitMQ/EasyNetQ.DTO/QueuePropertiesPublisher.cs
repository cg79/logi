using EasyNetQ.Topology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyNetQ.DTO
{
   public  class QueuePropertiesPublisher
    {
       public QueuePropertiesPublisher()
       {
 
       }
       private string busName = "host=localhost";
       public string BusName
       {
           get { return busName; }
           set { busName = value; }
       }
       
       public ExchangeProperties ExchangeProperties { get; set; }
       public string RoutingKey { get; set; }

       //Durable queues remain active when a server restarts
       bool durable = true;
       public bool Durable
       {
           get { return durable; }
           set { durable = value; }
       }

       bool autoDelete = true;
       //If set, the queue is deleted when all consumers have finished using it.
       public bool AutoDelete
       {
           get { return autoDelete; }
           set { autoDelete = value; }
       }
       ConnectionFactoryProperties connectionFactory = null;
       public ConnectionFactoryProperties ConnectionFactory 
       {
           get 
           {
               if (connectionFactory == null)
               {
                   connectionFactory = new ConnectionFactoryProperties();
               }
               return connectionFactory;
           }
           set 
           {
               connectionFactory = value;
           }
       }
      

    }
}
