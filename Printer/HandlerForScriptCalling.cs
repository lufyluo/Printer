using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Printer.Print;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Printer
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public class HandlerForScriptCalling
    {
        private Window _mainWindow;
        public HandlerForScriptCalling(Window mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public void HandleTest(string p)
        {
            MessageBox.Show(p);
        }
        public void HandleTest1(string p)
        {
            MessageBox.Show(p);
        }
        public void HandleTest2(string p)
        {
            MessageBox.Show(p);
        }

        private void SetDefaultPrinter()
        {
            Externs.SetDefaultPrinter("");
        }

    }
}
