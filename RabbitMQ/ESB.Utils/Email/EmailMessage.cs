using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESB.Utils.Email
{
    public class EmailMessage 
    {
        public string Subject { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Body { get; set; }
        Encoding bodyEncoding = Encoding.UTF8;
        public Encoding BodyEncoding 
        {
            get { return bodyEncoding; }
            set { bodyEncoding = value; }
        }
        bool isBodyHtml = true;
        public bool IsBodyHtml 
        {
            get { return isBodyHtml; }
            set { isBodyHtml = value; } 
        }


        public string TemplatePath
        {
            get;
            set;
        }

        public string XmlAsString
        {
            get;
            set;
        }
    }
}
