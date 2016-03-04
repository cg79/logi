﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESB.Utils.Email
{
    public class EmailSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
        public string FromEmailAddress { get; set; }
        public string FromEmailName { get; set; }
    }
}
