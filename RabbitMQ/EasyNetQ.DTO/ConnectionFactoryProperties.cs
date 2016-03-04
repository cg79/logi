using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyNetQ.DTO
{
    public class ConnectionFactoryProperties
    {
        string hostName = "localhost";

        public string HostName
        {
            get { return hostName; }
            set { hostName = value; }
        }

        int port = 5672;

        public int Port
        {
            get { return port; }
            set { port = value; }
        }
        string userName = "guest";

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        string password = "guest";

        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        string virtualHost = "/";

        public string VirtualHost
        {
            get { return virtualHost; }
            set { virtualHost = value; }
        }


    }
}
