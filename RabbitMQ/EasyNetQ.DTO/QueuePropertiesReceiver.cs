using EasyNetQ.Topology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyNetQ.DTO
{
    public class QueuePropertiesReceiver : QueuePropertiesPublisher
    {
       public QueuePropertiesReceiver()
       {
 
       }
       
       public string QueueName { get; set; }

    }
}
