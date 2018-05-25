using System.Collections.Generic;
using Printer.Framework.Printer;
using SelectItem = Printer.Core.Printer.Model.SelectItem;

namespace Printer.Core.Printer
{
    public class Finder
    {
        public static List<SelectItem> GetAllPrinters()
        {
            return PrinterBase.GetAllPrinters();
        }
    }
}
