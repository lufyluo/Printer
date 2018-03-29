using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Printer.Framework.Printer.ReceiptPrinterESC
{
    [StructLayout(LayoutKind.Sequential)]

    public struct OVERLAPPED

    {

        int Internal;

        int InternalHigh;

        int Offset;

        int OffSetHigh;

        int hEvent;

    }
    [StructLayout(LayoutKind.Sequential)]

    public struct PRINTER_DEFAULTS

    {



        public int pDatatype;



        public int pDevMode;



        public int DesiredAccess;



    }



    /// <summary>

    /// 对齐方式

    /// </summary>

    public enum eTextAlignMode

    {

        Left = 0,

        Middle = 1,

        Right = 2

    }

}
