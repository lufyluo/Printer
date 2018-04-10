using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Printer.Framework.Config;
using Printer.Framework.Printer.ServiceTickPrinter.Model;

namespace Printer.Framework.Printer.ServiceTickPrinter
{
    public class LogisticsPrinter : PrinterBase
    {
        private readonly string _printer = "ReceiptPrinter";
        public LogisticsReceiptBound logisticsReceiptBound = new LogisticsReceiptBound();

        public LogisticsReceiptBound getLogisticsReceiptBound()
        {
            return logisticsReceiptBound;
        }

        public void setLogisticsReceiptBound(string value)
        {
            logisticsReceiptBound = JsonConvert.DeserializeObject<LogisticsReceiptBound>(value);
        }
        public string print(string value)
        {
            try
            {
                setLogisticsReceiptBound(value);
                NewUsb.FindUSBPrinter();
                var result = NewUsb.LinkUSB(int.Parse(ConfigManager.GetSetting(_printer)));
                if (result)
                {
                    SendData2USB(PrinterCmdUtils.reset());
                    LoadPOSDll.POS_SetLineSpacing(130);
                    MainPrint("存根联");
                    SpecialState1();
                    SendData2USB(PrinterCmdUtils.reset());
                    SendData2USB(PrinterCmdUtils.nextLine(1));
                    LoadPOSDll.POS_SetLineSpacing(100);
                    MainPrint("客户联");
                    SpecialState2();
                    LoadPOSDll.POS_FeedLines(2);
                    SendData2USB(enddata);
                    SendData2USB(" \r\n");
                    SendData2USB(PrinterCmdUtils.feedPaperCutAll());
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

        private void MainPrint(string type)
        {
            PrintTitle(logisticsReceiptBound.Title+$"({type})");
            PrintBar(logisticsReceiptBound.BarCode,false);
            //SendData2USB(logisticsReceiptBound.BarCode);
            SendData2USB(PrinterCmdUtils.reset());
            SendData2USB(PrinterCmdUtils.nextLine(1));
            //SendData2USB(enddata);
            PrintHead();
            PrintBody();
        }

        private void PrintHead()
        {
            var maxLengthString = $"托运时间:{logisticsReceiptBound.ConsignmentDate.ToString("yyyy-MM-dd")}";
            PrintTitle($"{logisticsReceiptBound.Goods}");
            SendData2USB($"发站:{logisticsReceiptBound.StartStation}".GetAdjustedString(maxLengthString));
            PrintTitleSp(2);
            SendData2USB($"电话:{logisticsReceiptBound.Mobile}");
            SendData2USB(enddata);
            SendData2USB($"到站:{logisticsReceiptBound.Terminus}".GetAdjustedString(maxLengthString));
            PrintTitleSp(2);
            SendData2USB($"电话:{logisticsReceiptBound.TelPhone}");
            SendData2USB(enddata);
            SendData2USB(maxLengthString);
            PrintTitleSp(2);
            SendData2USB($"开单时间:{DateTime.Now.ToString("yyyy-MM-dd hh:mm")}");
            SendData2USB(enddata);
            SendData2USB($"打印时间:{logisticsReceiptBound.PrintDate.ToString("yyyy-MM-dd hh:mm")}");
            SendData2USB(enddata);
            PrintLine();
        }
        private void PrintBody()
        {
            var maxLengthString = $"托运时间:{logisticsReceiptBound.ConsignmentDate.ToString("yyyy-MM-dd")}";
            PrintTitle($"收货:{logisticsReceiptBound.Reciever} {logisticsReceiptBound.RecieverPhone.ToString()} \r\n", false);
            SendData2USB($"{("发货人:"+logisticsReceiptBound.Sender).GetAdjustedString(maxLengthString)}");
            //PrintTitleSp(2);
            SendData2USB($"电话:{logisticsReceiptBound.SenderPhone}");
            SendData2USB(enddata);
            PrintCommonTable(logisticsReceiptBound.GoodsDetails);
            NextLine();
            PrintLine();
            SendData2USB($"送货费:{logisticsReceiptBound.DeliveryFee.ToString().GetAdjustedStringByCols(12)}");
            //PrintTitleSp(12);
            SendData2USB($"现付:{logisticsReceiptBound.Pay.ToString().GetAdjustedStringByCols(12)}");
            //PrintTitleSp(12);
            SendData2USB($"回付:{logisticsReceiptBound.PayBack} \r\n");
            SendData2USB(PrinterCmdUtils.nextLine(1));
            SendData2USB($"开票备注:{logisticsReceiptBound.BillingStaffRemarks} \r\n");
            SendData2USB(PrinterCmdUtils.nextLine(1));
            SendData2USB(PrinterCmdUtils.setLineHeight(25));
            PrintTitle($"提付:{logisticsReceiptBound.TakePay} \r\n", false);
            SendData2USB(PrinterCmdUtils.setLineHeight(25));
            PrintTitle($"代收款:{logisticsReceiptBound.Collection} \r\n",false);
            PrintTitle($"到付应收:{logisticsReceiptBound.FinalPay}", false);
           
        }

        private void SpecialState1()
        {
            PrintLine();
            LoadPOSDll.POS_SetLineSpacing(85);
            SendData2USB("特别声明:本单有效期一个月\r\n");
            SendData2USB(enddata);
            SendData2USB(" \r\n");
            SendData2USB(PrinterCmdUtils.nextLine(1));
            SendData2USB("签收人签章:\r\n");
            SendData2USB(" \r\n");
            SendData2USB(enddata);
            SendData2USB(PrinterCmdUtils.nextLine(1));
            SendData2USB("签收人证件号:\r\n");
            SendData2USB(" \r\n");
            SendData2USB(enddata);
            SendData2USB(PrinterCmdUtils.nextLine(1));
            PrintCutTipLine();
        }
        private void SpecialState2()
        {
            PrintLine();
            SendData2USB("特别声明:");
            SendData2USB(PrinterCmdUtils.nextLine(1));
            SendData2USB("1.本公司无审核所托运货物价值的能力和义务，赔偿标准亦以托运人支付的运费作为赔偿依据，未保价货物，货损最高按部分运费的5倍理赔，丢失最高按部分运费的10倍理赔(多件以平均丢失最高按部分运费的10倍理赔)");

            SendData2USB(PrinterCmdUtils.nextLine(1));
            SendData2USB("2.凡在我公司托运有代收款业务的货物，请收贷方及时提货，公司仅免费保存3天，7天后将原货返回成都");
            SendData2USB(PrinterCmdUtils.nextLine(1));
            SendData2USB("3.服务范围:本公司成都收货门店起至到站地卸货门止");
            SendData2USB(PrinterCmdUtils.nextLine(1));
            SendData2USB("服务期限:本单有效期1个月");
        }
        private void PrintCutTipLine()
        {
            SendData2USB("-------------------请沿次线撕开-----------------");
            SendData2USB(PrinterCmdUtils.nextLine(1));
        }
    }
}
