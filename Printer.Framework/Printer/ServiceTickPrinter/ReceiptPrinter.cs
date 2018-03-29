using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Printer.Framework.Config;
using Printer.Framework.Printer.ServiceTickPrinter.Model;

namespace Printer.Framework.Printer.ServiceTickPrinter
{
    public class ReceiptPrinter:PrinterBase
    {
        private readonly string _printer = "ReceiptPrinter";
        public ServiceReceiptBound serviceReceipt = new ServiceReceiptBound();
        public ServiceReceiptBound getServiceReceipt()
        {
            return serviceReceipt;
        }
        public void setServiceReceipt(string value)
        {
            serviceReceipt = JsonConvert.DeserializeObject<ServiceReceiptBound>(value);
        }
        public string JsonServiceReceipt { get; set; }
        public void serviceTickPrint(string value)
        {
            setServiceReceipt(value);
            
            NewUsb.LinkUSB(int.Parse(ConfigManager.GetSetting(_printer)));
            for (int i = 0,l = serviceReceipt.Orders.Count; i < l; i+=20)
            {
                PrintHead(i+1);
                PrintTable(i);
            }
        }

        private void PrintHead(int page)
        {
            SendData2USB(_shiftsize);
            SendData2USB(_kanjiMode);
            SendData2USB(_boldAndCenter);
            SendData2USB(serviceReceipt.Title);
            SendData2USB(enddata);
            SendData2USB($"时间：{serviceReceipt.Date}");
            PrintTitleSp();
            SendData2USB($"当前第：{page}页/共{serviceReceipt.Total}页");
            SendData2USB(enddata);
            SendData2USB($"门店：{serviceReceipt.Store}");
            PrintTitleSp();
            SendData2USB($"线路：{serviceReceipt.Way}");
            SendData2USB(enddata);
        }

        private void PrintTable(int i)
        {
            PrintTableHead();
            SendData2USB(PrinterCmdUtils.underlineWithOneDotWidthOn());
        }

        private void PrintTableHead()
        {
            SendData2USB(PrinterCmdUtils.underlineWithOneDotWidthOn());
            SendData2USB("|");

        }

        private void PrintTitleSp()
        {
            SendData2USB(new byte[]
            {
                PrinterCmdUtils.SP, PrinterCmdUtils.SP,
                PrinterCmdUtils.SP, PrinterCmdUtils.SP,
                PrinterCmdUtils.SP, PrinterCmdUtils.SP,
                PrinterCmdUtils.SP, PrinterCmdUtils.SP,
                PrinterCmdUtils.SP, PrinterCmdUtils.SP
            });
        }

        public void transportTickPrint()
        {

        }
       
    }
}
