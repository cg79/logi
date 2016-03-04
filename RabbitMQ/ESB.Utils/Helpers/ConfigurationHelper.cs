using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESB.Utils.Config
{
    public class ConfigurationHelper
    {
        public static T ReadConfigurationKey<T>(string key, T defaultValue = default(T))
        {
            T rez = defaultValue;
            if (ConfigurationManager.AppSettings.AllKeys.Any(k => k == key))
            {
                rez = (T)Convert.ChangeType(ConfigurationManager.AppSettings[key], typeof(T));
                return rez;
            }
            return defaultValue;
        }

        public static string ReadConnectionString(string key)
        {
            if (ConfigurationManager.ConnectionStrings[key] != null)
            {
                return ConfigurationManager.ConnectionStrings[key].ToString();
            }
            return string.Empty;
        }
    }
}
