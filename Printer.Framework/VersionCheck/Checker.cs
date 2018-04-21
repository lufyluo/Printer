using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Printer.Framework.Config;
using Printer.Framework.VersionCheck.Models;
using Printer.Framework.WebProxy;

namespace Printer.Framework.VersionCheck
{
    public class Checker
    {
        public static ServerConfig _config;
        public static object lockObj = new object();

        public static ServerConfig GetServerConfig()
        {
            if (_config == null)
            {
                lock (lockObj)
                {
                    FileStream stream = new FileStream(ConfigManager.GetSetting("ConfigFileAddress"), FileMode.Open);
                    BinaryFormatter bFormat = new BinaryFormatter();
                    var configs = (List<ServerConfig>)bFormat.Deserialize(stream);
                    _config = configs.LastOrDefault();
                }
                
            }
            return _config;
        }

        public static bool IsNeedUpdate()
        {
            try
            {
                var currentVersion = ConfigManager.GetSetting("Version");
                var serverAddr = ConfigManager.GetAppConfig("ConfigFileAddress");
                var url = HttpHelper.GetResult(serverAddr);
                var version = JsonConvert.DeserializeObject<VersionResult>(url);
                ConfigManager.UpdateSetting("DownloadSite", version.WIN_VERSION_URL);
                ConfigManager.UpdateSetting("Version", version.WIN_VERSION);
                if (!string.IsNullOrEmpty(version.WIN_VERSION_URL))
                {
                    return version.WIN_VERSION != currentVersion;
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            
        }

        private static string GetUrl(string result)
        {

            int i = result.IndexOf("http");
            int j = result.IndexOf(".zip");
            return result.Substring(i, j - i + 2);
        }
    }

    public class ServerConfig
    {
        public int server_id { get; set; }
        public string server_name { get; set; }
        public string server_addr { get; set; }
        public string resource_addr { get; set; }
    }
}
