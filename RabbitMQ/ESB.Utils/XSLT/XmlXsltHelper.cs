using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace ESB.Utils.XSLT
{
    public static class XmlXsltHelper
    {

        public static string GenerateHtml(this string request, string sXslPath)
        {
            try
            {
                string retValue = string.Empty;
                //load the Xml doc
                XPathDocument myXPathDoc = new XPathDocument(new StringReader(request));

                XslTransform myXslTrans = new XslTransform();

                //load the Xsl 
                myXslTrans.Load(sXslPath);


                using (MemoryStream myWriter = new MemoryStream())
                {

                    //do the actual transform of Xml
                    myXslTrans.Transform(myXPathDoc, null, myWriter);

                    retValue = Encoding.UTF8.GetString(myWriter.GetBuffer());
                    myWriter.Close();
                }
                return retValue;
            }
            catch (Exception e)
            {

                Console.WriteLine("Exception: {0}", e.ToString());
            }
            return string.Empty;
        }
    }
}
