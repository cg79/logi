using ESB.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyNetQ.DTO
{
    public class Response : BaseRequestResponse
    {
        public string JsonResponse { get; set; }
        public string Error { get; set; }
        
    }
}
