using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.IO;

namespace ESB.Utils.Serializers
{
    public static class JSONHelper
    {
        public static string ToJSON(this object obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj);
        }

        public static T JsonDeserialize<T>(this string json)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            T retValue = serializer.Deserialize<T>(json);
            return retValue;
        }

        public static T Clone<T>(this object obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(obj);
            T retValue = serializer.Deserialize<T>(json);
            return retValue;
        }
    }

}
