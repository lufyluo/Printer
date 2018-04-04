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

    /// <summary>
    /// 门店大票清单
    /// </summary>
    public class ReceiptPrinter:PrinterBase
    {
        private readonly string _printer = "ReceiptPrinter";
        public ServiceReceiptBound serviceReceipt = new ServiceReceiptBound();
        private readonly int pageMax = 20;
        public ServiceReceiptBound getServiceReceipt()
        {
            return serviceReceipt;
        }
        public void setServiceReceipt(string value)
        {
            serviceReceipt = JsonConvert.DeserializeObject<ServiceReceiptBound>(value);
        }
        public string JsonServiceReceipt { get; set; }
        public string print(string value)
        {
            try
            {
                setServiceReceipt(value);
                NewUsb.FindUSBPrinter();
                var result = NewUsb.LinkUSB(int.Parse(ConfigManager.GetSetting(_printer)));
                if (result)
                {
                    SendData2USB(PrinterCmdUtils.reset());
                    for (int i = 0, p = 1, l = serviceReceipt.Orders.Count; i < l; i += pageMax, p++)
                    {
                        PrintHead(p);
                        PrintTable(p);
                        SendData2USB("\r\n");
                        SendData2USB(PrinterCmdUtils.lineSpace(60));
                        LoadPOSDll.POS_FeedLines(2);
                        SendData2USB(PrinterCmdUtils.feedPaperCutAll());
                    }
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

        private void PrintHead(int page)
        {
            PrintTitle();
            SendData2USB($"时间：{serviceReceipt.Date.ToString("yyyy-MM-dd")}");
            PrintTitleSp();
            SendData2USB($"当前第{page}页/共{Math.Ceiling((decimal) serviceReceipt.Orders.Count/pageMax)}页");
            SendData2USB(enddata);
            SendData2USB($"门店：{serviceReceipt.Store.GetAdjustedString(serviceReceipt.Date.ToString("yyyy-MM-dd"))}");
            PrintTitleSp();
            SendData2USB($"线路：{serviceReceipt.Way}");
            SendData2USB(enddata);
        }

        private void PrintTitle()
        {
            SendData2USB(_boldAndCenter);
            SendData2USB(serviceReceipt.Title);
            SendData2USB(PrinterCmdUtils.nextLine(1));
            SendData2USB(PrinterCmdUtils.reset());
            SendData2USB(enddata);
        }

        private void PrintTable(int i)
        {
            PrintTableHead();
            PrintTableBody(i);
        }

        private void PrintTableBody(int page)
        {
            var indexPage = page - 1;
            var orders = serviceReceipt.Orders.Skip(indexPage * pageMax).Take(pageMax).ToList();
            for (int i = 0,l = orders.Count(); i < l; i++)
            {
                var item = orders[i];
                var index = indexPage * pageMax + i + 1;
                SendData2USB($@"┃{index.GetIntFix()}┃{item.NoBytes}┃{item.RecieverBytes}┃{item.RecieverPhoneBytes}┃");
                NextLine();
                SendData2USB("┃  ┣━━━━━━━━╋━━━━╋━━━━━━┫");
                NextLine();
                SendData2USB($"┃  ┃{item.TypeBytes}┃{item.TakePayBytes}┃{item.CollectionBytes}┃");
                NextLine();
                if (i == (l - 1))
                {
                    SendData2USB("┗━┻━━━━━━━━┻━━━━┻━━━━━━┛");
                    break;
                }
                SendData2USB("┣━╋━━━━━━━━╋━━━━╋━━━━━━┫");

            }
        }

        private void PrintTableHead()
        {
            SendData2USB("┏━┳━━━━━━━━┳━━━━┳━━━━━━┓");
            NextLine();
            SendData2USB("┃序┃    货物编号    ┃ 收货人 ┃ 收货人电话 ┃");
            NextLine();
            SendData2USB("┣━╋━━━━━━━━╋━━━━╋━━━━━━┫");
            NextLine();

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
