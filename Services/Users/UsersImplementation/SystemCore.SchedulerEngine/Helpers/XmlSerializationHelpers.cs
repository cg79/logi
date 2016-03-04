using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace SchedulerEngine
{
    /// <summary>
    /// XML serialization extension methods
    /// </summary>
    public static class XmlExtensionMethods
    {
        /// <summary>
        /// returns the XML associated with the current object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToXml(this object obj)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

//Add an empty namespace and empty value
ns.Add("", "");

            XmlSerializer s = new XmlSerializer(obj.GetType());
            using (StringWriter writer = new StringWriter())
            {
                s.Serialize(writer, obj,ns);
                return writer.ToString();
            }
        }
        /// <summary>
        /// create a new T object based on the input XML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T FromXml<T>(this string data)
        {
            XmlSerializer s = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(data))
            {
                object obj = s.Deserialize(reader);
                return (T)obj;
            }
        }
    }
}
