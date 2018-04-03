using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarcodeLib;
using Newtonsoft.Json;
using Printer.Framework.Config;
using Printer.Framework.Printer.ServiceTickPrinter.Model;

namespace Printer.Framework.Printer.ServiceTickPrinter
{
    /// <summary>
    /// 货物运输单
    /// </summary>
    public class TransportReceiptPrinter : PrinterBase
    {
        private readonly string _printer = "ReceiptPrinter";
        public TransportReceiptBound transportReceipt = new TransportReceiptBound();

        public TransportReceiptBound getTransportReceipt()
        {
            return transportReceipt;
        }

        public void setTransportReceipt(string value)
        {
            transportReceipt = JsonConvert.DeserializeObject<TransportReceiptBound>(value);
        }

        public void print(string value)
        {
            setTransportReceipt(value);
            NewUsb.FindUSBPrinter();
            NewUsb.LinkUSB(int.Parse(ConfigManager.GetSetting(_printer)));
            SendData2USB(PrinterCmdUtils.reset());
            LoadPOSDll.POS_SetLineSpacing(130);
            PrintTitle(transportReceipt.Title);
            PrintBar(transportReceipt.BarCode);
            //SendData2USB(transportReceipt.BarCode);
            SendData2USB(PrinterCmdUtils.reset());
            SendData2USB(PrinterCmdUtils.nextLine(1));
            //SendData2USB(enddata);
            PrintHead();
            PrintBody();
            LoadPOSDll.POS_FeedLines(2);
            SendData2USB(PrinterCmdUtils.feedPaperCutAll());
        }
        private void PrintHead()
        {
            var maxLengthString = $"托运时间:{transportReceipt.ConsignmentDate.ToString("yyyy-MM-dd")}";
            PrintTitle($"货号:{transportReceipt.Goods}");
            SendData2USB($"发站:{transportReceipt.StartStation}".GetAdjustedString(maxLengthString));
            PrintTitleSp(2);
            SendData2USB($"电话:{transportReceipt.Mobile}");
            SendData2USB(enddata);
            SendData2USB($"到站:{transportReceipt.Terminus}".GetAdjustedString(maxLengthString));
            PrintTitleSp(2);
            SendData2USB($"电话:{transportReceipt.TelPhone}");
            SendData2USB(enddata);
            SendData2USB(maxLengthString);
            PrintTitleSp(2);
            SendData2USB($"打印时间:{DateTime.Now.ToString("yyyy-MM-dd hh:mm")}");
            SendData2USB(enddata);
            PrintLine();
           
        }

      

        private void PrintBody()
        {
            var maxLengthString = $"托运时间:{transportReceipt.ConsignmentDate.ToString("yyyy-MM-dd")}";
            PrintTitle($"收货:{transportReceipt.Reciever} {transportReceipt.RecieverPhone.ToString()}",false);
            SendData2USB($"发货人:{transportReceipt.Sender.GetAdjustedString(maxLengthString)}");
            PrintTitleSp(2);
            SendData2USB($"电话:{transportReceipt.SenderPhone}");
            SendData2USB(enddata);
            PrintCommonTable(transportReceipt.GoodsDetails);
            NextLine();
            PrintLine();
            SendData2USB($"运费:{transportReceipt.FreightFee}");
            PrintTitleSp(4);
            SendData2USB($"送货费:{transportReceipt.DeliveryFee}");
            PrintTitleSp(4);
            SendData2USB($"接货费:{transportReceipt.RecieveFee}");
            SendData2USB(PrinterCmdUtils.nextLine(2));
            SendData2USB($"保险费:{transportReceipt.SecureFee}(客户{(transportReceipt.IsProtected?"要":"不")}保价)");
            SendData2USB(PrinterCmdUtils.nextLine(2));
            PrintTitle($"总费用:{transportReceipt.TotalFee}",false);
            SendData2USB($"大写:{transportReceipt.Money}(提付:{transportReceipt.TakePay})");
            SendData2USB(PrinterCmdUtils.nextLine(1));
            PrintTitle($"代收款:{transportReceipt.Collection}",false);
            SendData2USB($"开票人:{transportReceipt.BillingStaff}");
            SendData2USB(PrinterCmdUtils.nextLine(1));
            SendData2USB($"验证码:{transportReceipt.VerificationCode}");
            SendData2USB(PrinterCmdUtils.nextLine(1));
            SendData2USB($"开票人备注:{transportReceipt.BillingStaffRemarks}");
            SendData2USB(PrinterCmdUtils.nextLine(1));
            PrintLine();
            SendData2USB(PrinterCmdUtils.lineSpace(120));
            PrintTitle("特别说明:", false);
            SendData2USB(PrinterCmdUtils.nextLine(1));
            PrintSpecialState();
            SendData2USB("托运人（托运委托人）签字:\r\n");
            SendData2USB(PrinterCmdUtils.nextLine(3));
            SendData2USB("请妥善保存你的运输单。\r\n");
            SendData2USB($"客服电话:{transportReceipt.CustomerServicePhone}\r\n");
            SendData2USB($"公司网址:{transportReceipt.CompanyWebSite}\r\n");
            SendData2USB($"发站地址:{transportReceipt.StartStationAddr}\r\n");
            SendData2USB($"到站地址:{transportReceipt.TerminusAddr}\r\n");
        }

        private void PrintSpecialState()
        {
            SendData2USB("1.按托运货物重量或方量（非价值）收取运费。\r\n");
            SendData2USB("2.进入保价的贵重货物将以保价价值100%给予赔付，但保价不得超过货物时间价格。\r\n");
            SendData2USB("3.未参加保价货物，若丢失或损坏按运费的5至10倍赔付，多件以平均单价为准。\r\n");
            SendData2USB("4.易损、易碎品只保丢失，破损不负责赔偿。\r\n");
            SendData2USB("有生命力的动植物，易变质食品，不负责赔偿\r\n");
            SendData2USB("在我公司的代收款业务的货物，除现金提货外，只尽代收、证实义务，不承担其它责任。\r\n");
            SendData2USB("服务时限:本单有效期1个月。\r\n");
            SendData2USB("托运人在充分阅读理解本运输单及门店《委托运输合同条款》内容基础上，取得本运输单，即使未签字，一样视为对本条款认可，且没有补充，自愿承担相应责任。\r\n");
            SendData2USB(PrinterCmdUtils.nextLine(3));
        }

        private void PrintTitleSp()
        {
            SendData2USB(new byte[]
            {
                PrinterCmdUtils.SP, PrinterCmdUtils.SP,
                PrinterCmdUtils.SP, PrinterCmdUtils.SP,
                PrinterCmdUtils.SP, PrinterCmdUtils.SP,
                PrinterCmdUtils.SP, PrinterCmdUtils.SP
            });
        }
    }

    
}
