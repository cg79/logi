using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESB.Utils.Reflection
{
    public class LogisticMethod
    {
        public string Library { get; set; }
        public string Namespace { get; set; }
        public string Method { get; set; }
        public string  JSON { get; set; }
        public Guid RequestGuid { get; set; }
        public string ChannelId { get; set; }
        public string RKey { get; set; }
    }
}
