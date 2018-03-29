namespace Printer.Framework.Printer
{
    public abstract class PrinterBase
    {
        protected readonly libUsbContorl.UsbOperation NewUsb = new libUsbContorl.UsbOperation();
        protected readonly byte[] _shiftsize = { 0x1d, 0x57, 0xd0, 0x01 };//偏移量
        protected readonly byte[] _kanjiMode = { 0x1c, 0x26 };//汉字模式
        protected readonly byte[] _boldAndCenter = { 0x1b, 0x61, 0x01, 0x1b, 0x21, 0x30, 0x1c, 0x57, 0x01 };
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
    }
}
