using System;
using System.Collections.Generic;
using System.Linq;
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
        public static bool IsNeedUpdate()
        {
            var currentVersion = ConfigManager.GetSetting("Version");
            var checkService = ConfigManager.GetSetting("VersionCheckService");
            var url = HttpHelper.GetResult($"{checkService}{currentVersion}");
            var version = JsonConvert.DeserializeObject<VersionResult>(url);
            ConfigManager.UpdateSetting("DownloadSite", version.Url);
            ConfigManager.UpdateSetting("Version", version.Version);
            if (string.IsNullOrEmpty(version.Url))
            {
                return false;
            }
            return true;
        }

        private static string GetUrl(string result)
        {

            int i = result.IndexOf("http");
            int j = result.IndexOf(".zip");
            return result.Substring(i, j - i + 2);
        }
    }
}
