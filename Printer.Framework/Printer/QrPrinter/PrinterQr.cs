using System.Windows.Forms;
using Printer.Framework.Printer.QrPrinter.Model;

namespace Printer.Framework.Printer.QrPrinter
{
    public class PrinterQr:PrinterBase, IQrPrinter
    {
        public void SetPrinter(string printerName)
        {
            throw new System.NotImplementedException();
        }

        public void Print(QrPrintParam value)
        {
            throw new System.NotImplementedException();
        }

        public void show(string msg)
        {
            MessageBox.Show(msg);
        }
    }
}
