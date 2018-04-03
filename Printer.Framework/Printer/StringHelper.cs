using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printer.Framework.Printer
{
    public static class StringHelper
    {
        public static string GetAdjustedString(this string adjustString, string standardString)
        {
            var adjustBytes = Encode(adjustString);
            var standardBytes = Encode(standardString);
            var result = new byte[standardBytes.Length];
            if (adjustBytes.Length < standardBytes.Length)
            {
                CopyRange(result, adjustBytes);
                for (int i = 0, l = standardBytes.Length - adjustBytes.Length; i < l; i++)
                {
                    result[i + adjustBytes.Length] = PrinterCmdUtils.SP;
                }
            }
            return Decode(result);
        }
        /// <summary>
        /// 指定占宽度
        /// </summary>
        /// <param name="adjustString"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static string GetAdjustedString(this string adjustString, int width)
        {
            var adjustBytes = Encode(adjustString);
            var result = new byte[width];
            CopyRange(result, adjustBytes);
            if (adjustBytes.Length < width)
            {
                for (int i = 0, l = width - adjustBytes.Length; i < l; i++)
                {
                    result[i + adjustBytes.Length] = PrinterCmdUtils.SP;
                }
            }
            return Decode(result);
        }
        public static string GetIntFix(this int adjustInt)
        {
            return adjustInt<10? adjustInt+" ": adjustInt.ToString();
        }

        private static byte[] CopyRange(byte[] targetBytes, byte[] rangeBytes)
        {
            for (int i = 0; i < Math.Min(rangeBytes.Length, targetBytes.Length); i++)
            {
                targetBytes[i] = rangeBytes[i];
            }
            return targetBytes;
        }

        private static byte[] Encode(string param)
        {
            return Encoding.GetEncoding(54936).GetBytes(param);
        }
        private static string Decode(byte[] param)
        {
            
            return Encoding.GetEncoding(54936).GetString(param);
        }
    }
}
