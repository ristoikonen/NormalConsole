using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration.Memory;
//SecretManager
using System.IO;
using System.Xml;

namespace NormalConsole
{
    //public static class ConfigurationExtensions
    //{
    //    public static IConfigurationBuilder AddDemoDbProvider(
    //        this IConfigurationBuilder configuration, Action<DbContextOptionsBuilder> setup)
    //    {
    //        configuration.Add(new BuildConfig().Init());
    //        return configuration;
    //    }
    //}

    //public class ConfigCreator : IConfigurationSource
    //{
    //    public IConfigurationProvider Build(IConfigurationBuilder builder)
    //    {
    //        //throw new NotImplementedException();
    //        //return new 
    //    }
    //}

    public class AppSettingsConfigurationSource : FileConfigurationSource
    {
        public AppSettingsConfigurationSource()
        {
            Path = "web.config";
            ReloadOnChange = true;
            Optional = true;
            FileProvider = null;
        }

        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            EnsureDefaults(builder);
            return new AppSettingsConfigurationProvider(this);
        }
    }

    public class AppSettingsConfigurationProvider : FileConfigurationProvider
    {
        public AppSettingsConfigurationProvider(AppSettingsConfigurationSource source) : base(source) { }

        public override void Load(Stream stream)
        {
            try
            {
                Data = ReadAppSettings(stream);
            }
            catch
            {
                throw new FormatException("Failed to read from stream");
            }
        }
        private IDictionary<string, string> ReadAppSettings(Stream stream)
        {
            var data =
              new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            var doc = new XmlDocument();
            doc.Load(stream);

            var appSettings = doc.SelectNodes("/configuration/appSettings/add");

            foreach (XmlNode child in appSettings)
            {
                data[child.Attributes["key"].Value] = child.Attributes["value"].Value;
            }

            return data;
        }
    }
    // https://wildermuth.com/2018/04/15/Building-a-NET-Core-Configuration-Source
    public class BuildConfig
    {
        public BuildConfig()
        {
            ConfigRoot = Init();
            ConfigRootKVP = Init2();
        }

        public IConfigurationRoot Init()
        {
            return new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();

            // returns an object that implements the IConfigurationRoot/IConfiguration interface
        }
        public IConfigurationRoot Init2()
        {
            KvpList = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("Key1", "Value1"),
                new KeyValuePair<string, string>("Key2", "123"),
                new KeyValuePair<string, string>("Key3", "Red")
            };

            return new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddInMemoryCollection((IEnumerable<KeyValuePair<string, string>>)KvpList).Build();

            // ConfigurationBuilder returns an object that implements the IConfigurationRoot/IConfiguration interface
        }

        public string NameFromConfig { get { return ConfigRoot["name"]; } }

        public List<KeyValuePair<string, string>> KvpList { get; private set; }

        public IConfigurationRoot ConfigRoot { get; private set; }

        public IConfigurationRoot ConfigRootKVP { get; private set; }

    }


    // so this is the actual data stored - just your 'everyday' POCO
    public class MySettings
    {
        public string CaptionText { get; set; } = "BitPerson";
        public int MaxPersonsPerList { get; set; } = 20;
    }

}
