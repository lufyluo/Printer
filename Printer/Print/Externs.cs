using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Printer.Print
{
    class Externs
    {
        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(String Name); //调用win api将指定名称的打印机设置为默认打印机
    }
}
