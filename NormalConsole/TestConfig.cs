using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;



namespace NormalConsole
{

    //TODO:Add ConfigurationErrorsException handing?
    public static class AppsConfig
    {
        public static string GetByKeyOrDefault(string key, string defaultVal) 
        {
            // Using Contains as Get -method does mot distinguish wheter value is null or key not found => null too!
            return ConfigurationManager.AppSettings.AllKeys.Contains(key) ?
                ConfigurationManager.AppSettings.Get(key) : defaultVal;
        }

        public static NameValueCollection GetAppSettings()
        {
            return ConfigurationManager.AppSettings; 
        }

    }
}
