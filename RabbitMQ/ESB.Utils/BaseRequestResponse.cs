using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESB.Utils
{
    public class BaseRequestResponse
    {
        public string ChannelId { get; set; }
        public Guid RequestGuid { get; set; }
    }
}
