using System.Collections.Generic;
using System.Linq;
using Printer.Framework.Printer.ServiceTickPrinter.Model;

namespace Printer.Framework.Printer
{
    public abstract class PrinterBase
    {
        protected readonly libUsbContorl.UsbOperation NewUsb = new libUsbContorl.UsbOperation();
        protected readonly byte[] _shiftsize = { 0x1d, 0x57, 0xd0, 0x01 };//偏移量
        protected readonly byte[] _kanjiMode = { 0x1c, 0x26 };//汉字模式
        protected readonly byte[] _boldAndCenter = { 0x1b, 0x61, 0x01, 0x1b, 0x21, 0x30, 0x1c, 0x57, 0x01 };
        protected readonly byte[] _fontNomal = { 28, 87, 0 };
        protected readonly byte[] _fontBold = { 28, 87, 1, 27, 33, 48 };
        //protected readonly byte[] _fontBold = { 28, 87, 1 };
        protected readonly byte[] enddata = { 0x0a };//换行
        protected void SendData2USB(byte[] str)
        {
            NewUsb.SendData2USB(str, str.Length);
        }
        protected void SendData2USB(string str)
        {
            byte[] by_SendData = System.Text.Encoding.GetEncoding(54936).GetBytes(str);
            SendData2USB(by_SendData);
        }
        /// <summary>
        /// 换行并设置行间距最小
        /// </summary>
        protected void NextLine()
        {
            SendData2USB(PrinterCmdUtils.lineSpace(51));
            SendData2USB(PrinterCmdUtils.nextLine(1));
        }
        /// <summary>
        /// 打印标题
        /// </summary>
        /// <param name="tile"></param>
        protected void PrintTitle(string tile, bool isCenter = true)
        {
            if (isCenter)
            {
                SendData2USB(PrinterCmdUtils.alignCenter());
            }

            SendData2USB(_fontBold);
            SendData2USB(tile + "\r\n");
            SendData2USB(PrinterCmdUtils.reset());
            SendData2USB(PrinterCmdUtils.nextLine(1));
            if (isCenter) SendData2USB(enddata);
        }
        protected void PrintCommonTable(List<GoodsDetail> goodsList)
        {
            SendData2USB("┏━━━━━━━━━━┳━━━━━┳━━┳━━┓");
            NextLine();
            SendData2USB("┃        品名        ┃   包装   ┃件数┃总数┃");
            NextLine();
            if (goodsList != null)
            {
                PrintCommonTableBody(goodsList);
            }
            SendData2USB("┗━━━━━━━━━━┻━━━━━┻━━┻━━┛");
            NextLine();

        }
        protected void PrintLine()
        {
            SendData2USB("------------------------------------------------");
            NextLine();
        }
        protected void PrintBar(string code, bool isNeedCodeShown=true)
        {
            byte barCodeLength = (byte)code.Length;
            SendData2USB(PrinterCmdUtils.alignCenter());
            SendData2USB(new byte[] { 29, 104, 90 });//h
            SendData2USB(new byte[] { 29, 119, 3 });//w
            if (isNeedCodeShown)
                SendData2USB(new byte[] { 29, 72, 50 });
            SendData2USB(new byte[] { 29, 107 });
            SendData2USB(new byte[] { 73, barCodeLength, 123, 67 });
            SendData2USB("\r\n");
        }
        protected void PrintTitleSp(int n)
        {
            var byteArr = new byte[n];
            for (int i = 0; i < byteArr.Length; i++)
            {
                byteArr[i] = PrinterCmdUtils.SP;
            }
            SendData2USB(byteArr);
        }

        private void PrintCommonTableBody(List<GoodsDetail> goodsList)
        {
            foreach (var item in goodsList)
            {
                SendData2USB("┣━━━━━━━━━━╋━━━━━╋━━╋━━┫");
                SendData2USB($@"┃{item.TypeBytes}┃{item.PackageBytes}┃{item.CountBytes}┃{item.TotalBytes}┃");
            }
        }

    }
}
