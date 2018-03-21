using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Printer
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public class HandlerForScriptCalling
    {

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
    }
}
