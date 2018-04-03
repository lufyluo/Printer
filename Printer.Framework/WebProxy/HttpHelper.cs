using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Printer.Framework.WebProxy
{
    public class HttpHelper
    {
        /// <summary>
        ///  Get请求数据
        /// </summary>
        /// <param name="url">请求的URL地址</param>
        /// <returns></returns>
        public static string GetResult(string url)
        {
            WebClient wc = new WebClient();
            string s = wc.DownloadString(url);
            s = HttpUtility.UrlDecode(s);
            return s;
        }
    }
}
