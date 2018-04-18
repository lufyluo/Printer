using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printer.Framework.Config
{
    public class ConfigManager
    {
        public static readonly string File = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "yulaila.exe");

        public static string GetSetting(string strKey)
        {
            return ConfigurationManager.AppSettings[strKey];

        }

        /// <summary>
        /// update & Add
        /// </summary>
        /// <param name="newKey"></param>
        /// <param name="newValue"></param>
        public static void UpdateSetting(string newKey, string newValue)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(File);
            bool exist = false;
            foreach (string key in config.AppSettings.Settings.AllKeys)
            {
                if (key == newKey)
                {
                    exist = true;
                }
            }
            if (exist)
            {
                config.AppSettings.Settings.Remove(newKey);
            }
            config.AppSettings.Settings.Add(newKey, newValue);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        public static string GetAppConfig(string strKey)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(File);
            foreach (string key in config.AppSettings.Settings.AllKeys)
            {
                if (key == strKey)
                {
                    return config.AppSettings.Settings[strKey].Value.ToString();
                }
            }
            return null;
        }


    }
}
