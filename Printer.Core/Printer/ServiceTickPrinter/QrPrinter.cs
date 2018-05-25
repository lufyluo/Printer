using System;
using Newtonsoft.Json;
using Printer.Core.Printer.ServiceTickPrinter.Model;
using Printer.Framework.Config;
using Printer.Framework.Printer;
namespace Printer.Core.Printer.ServiceTickPrinter
{
    public class QrPrinter : PrinterBase
    {
        private readonly string _printer = "StickPrinter";
        public QrReceiptBound qrReceipt = new QrReceiptBound();
        public QrReceiptBound getQrReceipt()
        {
            return qrReceipt;
        }

        public void setQrReceipt(string value)
        {
            qrReceipt = JsonConvert.DeserializeObject<QrReceiptBound>(value);
        }

        public string print(string value)
        {
            try
            {
                setQrReceipt(value);
                NewUsb.FindUSBPrinter();
                if (NewUsb.LinkUSB(int.Parse(ConfigManager.GetSetting(_printer))))
                {
                    SendData2USB("SET CUTTER BATCH\r\n");
                    SendData2USB("CLS\r\n");
                    SendData2USB("SIZE 58 mm,40 mm\r\n");//标签尺寸
                    SendData2USB("GAP 2 mm,0 mm\r\n");//间距为0
                    SendData2USB("DENSITY 14\r\n");//打印浓度
                    SendData2USB("REFERENCE 0,0\r\n");
                    SendData2USB($"TEXT 35,15,\"TSS24.BF2\",0,2,2,\"{qrReceipt.Goods}\"\r\n");
                    //SendData2USB("DENSITY 7\r\n");//打印浓度
                    //SendData2USB("TEXT 15,95,\"TSS24.BF2\",0,1,1,\"到站：\"\r\n");
                    //SendData2USB($"TEXT 70,85,\"TSS24.BF2\",0,2,2,\"{qrReceipt.Terminus}\"\r\n");

                    //SendData2USB("TEXT 15,165,\"TSS24.BF2\",0,1,1,\"收货：\"\r\n");

                    SendData2USB($"TEXT 22,120,\"TSS24.BF2\",0,2,2,\"{qrReceipt.Reciever}\"\r\n");

                    //SendData2USB("TEXT 15,235,\"TSS24.BF2\",0,1,1,\"品名：\"\r\n");
                    PrintType(qrReceipt.Type);
                    //SendData2USB($"TEXT 35,225,\"TSS24.BF2\",0,1,1,\"{qrReceipt.Type}\"\r\n");

                    //SendData2USB($"TEXT 275,265,\"TSS24.BF2\",0,1,1,\"{qrReceipt.Company}\"\r\n");
                    PrintTerminus(qrReceipt.Terminus);

                    //SendData2USB($"QRCODE 255,85,L,6,A,0,\"{qrReceipt.Code}\"\r\n");
                    SendData2USB($"PRINT {qrReceipt.Times},1\r\n");
                    SendData2USB("ADJUST\r\n");
                    SendData2USB("GOTO START\r\n");
                    SendData2USB("EOP\r\n");
                    NewUsb.CloseUSBPort();
                    return JsonConvert.SerializeObject(new AciontResult()
                    {
                        Success = true
                    });
                }
                else
                {
                    return JsonConvert.SerializeObject(new AciontResult()
                    {
                        Success = false,
                        Desc = "未发现配置的打印机！"
                    });
                }
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new AciontResult()
                {
                    Success = false,
                    Desc = e.Message
                });
            }


        }

        private void PrintTerminus(string qrReceiptTerminus)
        {
            if (qrReceipt.Terminus.Length > 2)
            {
                SendData2USB($"TEXT 170,120,\"TSS24.BF2\",0,4,4,\"{qrReceipt.Terminus}\"\r\n");
            }
            else
            {
                SendData2USB($"TEXT 215,120,\"TSS24.BF2\",0,4,4,\"{qrReceipt.Terminus}\"\r\n");
            }
        }

        private void PrintType(string qrReceiptType)
        {
            int lineLength = 8;
            int startY = 200;
            for (int i = 0,line=0, l = qrReceiptType.Length; i <= l; line++)
            {
                if (i + lineLength > qrReceiptType.Length)
                {
                    SendData2USB($"TEXT 22,{startY + (line) * 25},\"TSS24.BF2\",0,1,1,\"{qrReceiptType.Substring((i), l - (i))}\"\r\n");
                    return;
                }
                //var adjustP = LineStringAjustNumeric(qrReceiptType, i + lineLength);
                SendData2USB($"TEXT 22,{startY + line * 25},\"TSS24.BF2\",0,1,1,\"{qrReceiptType.Substring(i, lineLength)}\"\r\n");
                i += lineLength;
            }
           
            //SendData2USB($"TEXT 35,225,\"TSS24.BF2\",0,1,1,\"{qrReceipt.Type}\"\r\n");
        }

        private  int LineStringAjustNumeric(string source, int start)
        {
            var index = 0;
            while (start + index< source.Length&&IsNumberic(source.Substring(start + index, 1)))
            {
                index++;
            }
            return index;
        }

        private bool IsNumberic(string oText)
        {
            try
            {
                int var1 = Convert.ToInt32(oText);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
