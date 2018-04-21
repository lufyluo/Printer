using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Printer.Framework.Config;
using Printer.Framework.Printer.ServiceTickPrinter.Model;
using ZXing;
using ZXing.Common;
using ZXing.QrCode.Internal;

namespace Printer.Framework.Printer.ServiceTickPrinter
{
    public class QrPrinter:PrinterBase
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
                    SendData2USB("DENSITY 7\r\n");//打印浓度
                    SendData2USB("REFERENCE 0,0\r\n");
                    SendData2USB($"TEXT 35,15,\"TSS24.BF2\",0,2,2,\"{qrReceipt.Goods}\"\r\n");
                    SendData2USB("TEXT 15,95,\"TSS24.BF2\",0,1,1,\"到站：\"\r\n");
                    SendData2USB($"TEXT 70,85,\"TSS24.BF2\",0,2,2,\"{qrReceipt.Terminus}\"\r\n");
                    SendData2USB("TEXT 15,165,\"TSS24.BF2\",0,1,1,\"收货：\"\r\n");
                    SendData2USB($"TEXT 70,155,\"TSS24.BF2\",0,2,2,\"{qrReceipt.Reciever}\"\r\n");
                    SendData2USB("TEXT 15,235,\"TSS24.BF2\",0,1,1,\"品名：\"\r\n");
                    SendData2USB($"TEXT 70,225,\"TSS24.BF2\",0,2,2,\"{qrReceipt.Type}\"\r\n");
                    SendData2USB($"TEXT 285,260,\"TSS24.BF2\",0,1,1,\"{qrReceipt.Company}\"\r\n");
                    SendData2USB($"QRCODE 255,100,L,6,A,0,\"{qrReceipt.Code}\"\r\n");
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
    }
}
