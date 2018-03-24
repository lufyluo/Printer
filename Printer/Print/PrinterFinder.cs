using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printer.Print
{
    class PrinterFinder
    {
        public static List<SelectItem> GetAllPrinters()
        {
            PrintDocument print = new PrintDocument();
            //string sDefault = print.PrinterSettings.PrinterName;//默认打印机名
            List<SelectItem> names = new List<SelectItem>();

            foreach (string sPrint in PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                var Item = new SelectItem()
                {
                    Value = sPrint
                };

                names.Add(Item);
            }
            return names;
        }
    }
}
