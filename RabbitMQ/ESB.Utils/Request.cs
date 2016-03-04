using ESB.Utils;
using ESB.Utils.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyNetQ.DTO
{
    public class Request : BaseRequestResponse
    {
        
        public string RKey { get; set; }
        public LogisticMethod DynamicMethod { get; set; }
        public string Json { get; set; }
    }
}
