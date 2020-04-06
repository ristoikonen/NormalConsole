using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace NormalConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            //Use custon AppsConfig app.config test
            
            Console.WriteLine($"Davis:{AppsConfig.GetByKeyOrDefault("Davis", "MISSING")}");
            Console.WriteLine($"Robinson:{AppsConfig.GetByKeyOrDefault("Robinson", "MISSING")}");

            var nvc = AppsConfig.GetAppSettings();


            BuildConfig bc = new BuildConfig();

            System.Configuration.Configuration config =
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None) as Configuration;
            var appseetss = config.AppSettings;
                        
            Console.WriteLine($"{bc.NameFromConfig}");

            //System.Configuration.Configuration config2 =
            //    ConfigurationManager.OpenMappedExeConfiguration(


            Console.ReadKey();


            Console.WriteLine($"{bc.NameFromConfig}");

            Console.ReadKey();
        }


    }
}
