using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printer.Framework.Printer
{
    public abstract class PrinterBase
    {
        public void SetPrinter(string printerName)
        {
            Externs.SetDefaultPrinter(printerName);
        }
    }
}
