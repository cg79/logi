using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ESB.Utils.XML
{
    public static class XmlHelper
    {


        /// <summary>
        /// Converts an object to XML
        /// </summary>
        /// <param name="Object">Object to convert</param>
        /// <param name="FileToSaveTo">File to save the XML to (optional)</param>
        /// <param name="EncodingUsing">Encoding that the XML should be saved/returned as (defaults to ASCII)</param>
        /// <returns>string representation of the object in XML format</returns>
        public static string ToXML(this object Object, Encoding EncodingUsing = null)
        {
            if (Object == null)
                throw new ArgumentNullException("Object");
            if (EncodingUsing == null)
                EncodingUsing = new ASCIIEncoding();
            string XML = "";
            using (MemoryStream Stream = new MemoryStream())
            {
                XmlSerializer Serializer = new XmlSerializer(Object.GetType());
                Serializer.Serialize(Stream, Object);
                Stream.Flush();
                XML = EncodingUsing.GetString(Stream.GetBuffer(), 0, (int)Stream.Position);
            }

            return XML;
        }

       
    }
}
