using System;
using Newtonsoft.Json;
using Printer.Core.Printer.ServiceTickPrinter.Model;
using Printer.Framework.Config;
using Printer.Framework.Printer;
namespace Printer.Core.Printer.ServiceTickPrinter
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

        public string print(string value)
        {
            try
            {
                setTransportReceipt(value);
                NewUsb.FindUSBPrinter();
                var state = NewUsb.LinkUSB(int.Parse(ConfigManager.GetSetting(_printer)));
                if (state)
                {
                    SendData2USB(Core.Printer.PrinterCmdUtils.reset());
                    SendData2USB(Core.Printer.PrinterCmdUtils.printNextLine(1));
                    PrintTitle(transportReceipt.Title);
                    SendData2USB(Core.Printer.PrinterCmdUtils.setLineHeight(20));
                    SendData2USB(Core.Printer.PrinterCmdUtils.printNextLine(1));
                    PrintBar(transportReceipt.BarCode,false);
                    SendData2USB(Core.Printer.PrinterCmdUtils.setLineHeight(20));
                    SendData2USB(Core.Printer.PrinterCmdUtils.printNextLine(1));
                    SendData2USB(Core.Printer.PrinterCmdUtils.setBold(1));
                    SendData2USB(Core.Printer.PrinterCmdUtils.alignCenter());
                    SendData2USB(transportReceipt.BarCode.ToString() +" \r\n");
                    SendData2USB(Core.Printer.PrinterCmdUtils.reset());
                    SendData2USB(Core.Printer.PrinterCmdUtils.setLineHeight(20));
                    SendData2USB(Core.Printer.PrinterCmdUtils.printNextLine(1));
                    PrintHead();
                    PrintBody();
                    SendData2USB(Core.Printer.PrinterCmdUtils.printNextLine(3));
                    SendData2USB(Core.Printer.PrinterCmdUtils.feedPaperCutAll());
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
                Console.WriteLine(e);
                throw;
            }

        }
        private void PrintHead()
        {

            var maxLengthString = $"托运时间:{transportReceipt.ConsignmentDate.ToString("yyyy-MM-dd")}";
            var exceptLent = $"托运{transportReceipt.ConsignmentDate.ToString("yyyy-MM-dd")}";
            PrintTitle($"{transportReceipt.Terminus}:{transportReceipt.Goods}");
            SendData2USB(Core.Printer.PrinterCmdUtils.setLineHeight(20));
            SendData2USB(Core.Printer.PrinterCmdUtils.printNextLine(1));
            SendData2USB(Core.Printer.PrinterCmdUtils.setLineHeight(18));
            SendData2USB(StringHelper.GetAdjustedString((string) $"发站:{transportReceipt.StartStation} ", maxLengthString));
            PrintTitleSp(2);
            SendData2USB($"电话:{transportReceipt.Mobile} \r\n");
            SendData2USB(enddata);
            SendData2USB($"到站:");
            SendData2USB(Core.Printer.PrinterCmdUtils.setBold(1));
            SendData2USB(StringHelper.GetAdjustedString((string) $"{transportReceipt.Terminus}", exceptLent));
            SendData2USB(Core.Printer.PrinterCmdUtils.setBold(0));
            PrintTitleSp(2);
            SendData2USB($"电话:");
            SendData2USB(Core.Printer.PrinterCmdUtils.setBold(1));
            SendData2USB($"{transportReceipt.TelPhone}" + " \r\n");
            SendData2USB(Core.Printer.PrinterCmdUtils.setBold(0));
            SendData2USB(enddata);
            SendData2USB(maxLengthString);
            PrintTitleSp(2);
            SendData2USB($"打印时间:{transportReceipt.PrintDate} \r\n");
            SendData2USB(enddata);
            PrintLine();

        }



        private void PrintBody()
        {
            var maxLengthString = $"托运时间:{transportReceipt.ConsignmentDate.ToString("yyyy-MM-dd")}";
            SendData2USB(Core.Printer.PrinterCmdUtils.setLineHeight(25));
            PrintTitle($"收货:{transportReceipt.Reciever} {transportReceipt.RecieverPhone.ToString()} \r\n", false);
            SendData2USB($"发货人:{StringHelper.GetAdjustedString(transportReceipt.Sender, maxLengthString)}");
            PrintTitleSp(2);
            SendData2USB($"电话:{transportReceipt.SenderPhone}");
            SendData2USB(enddata);
            PrintCommonTable(transportReceipt.GoodsDetails);
            NextLine();
            PrintLine();
            SendData2USB(Core.Printer.PrinterCmdUtils.setLineHeight(18));
            if (transportReceipt.IsNeedShowFreightFeeEtc)
            {
                SendData2USB($"运费:{StringHelper.GetAdjustedStringByCols((transportReceipt.FreightFee + transportReceipt.RebateFee).ToString(), (int) 12)}");
                SendData2USB($"送货费:{StringHelper.GetAdjustedStringByCols(transportReceipt.DeliveryFee.ToString(), (int) 12)}");
                SendData2USB($"接货费:{transportReceipt.RecieveFee}");
            }

            SendData2USB(Core.Printer.PrinterCmdUtils.nextLine(2));
            SendData2USB($"保险费:{transportReceipt.SecureFee}");
            SendData2USB(Core.Printer.PrinterCmdUtils.nextLine(2));
            SendData2USB(Core.Printer.PrinterCmdUtils.setLineHeight(18));
            if (transportReceipt.IsNeedShowFreightFeeEtc)
            {
                PrintTitle($"总费用:{transportReceipt.TotalFee} \r\n", false);
                SendData2USB(Core.Printer.PrinterCmdUtils.setLineHeight(18));
                SendData2USB($"大写:{transportReceipt.AmountInWords} \r\n");
                SendData2USB(Core.Printer.PrinterCmdUtils.nextLine(1));
                SendData2USB(Core.Printer.PrinterCmdUtils.setLineHeight(18));
            }
            
           
           
            PrintTitle($"代收款:{transportReceipt.Collection} \r\n", false);
            SendData2USB(Core.Printer.PrinterCmdUtils.setLineHeight(18));
            if (transportReceipt.Collection != "0")
            {
                SendData2USB($"打款账户:{transportReceipt.ShipperBankName}/{transportReceipt.ShipperBankCardAccount} \r\n");
                SendData2USB(Core.Printer.PrinterCmdUtils.setLineHeight(18));
            }
            SendData2USB($"开票人:{transportReceipt.BillingStaff} \r\n");
            SendData2USB(Core.Printer.PrinterCmdUtils.nextLine(1));
            SendData2USB($"验证码:{transportReceipt.PrintCheckCode} \r\n");
            SendData2USB(Core.Printer.PrinterCmdUtils.nextLine(1));
            SendData2USB(Core.Printer.PrinterCmdUtils.setLineHeight(24));
            SendData2USB($"开票人备注:{transportReceipt.BillingStaffRemarks} \r\n");
            SendData2USB(Core.Printer.PrinterCmdUtils.nextLine(1));
            PrintLine();
            SendData2USB(Core.Printer.PrinterCmdUtils.setLineHeight(18));
            PrintTitle("特别说明:", false);
            SendData2USB(Core.Printer.PrinterCmdUtils.nextLine(1));
            PrintSpecialState();
            SendData2USB(Core.Printer.PrinterCmdUtils.nextLine(1));
            SendData2USB("托运人（托运委托人）签字: \r\n");
            SendData2USB(Core.Printer.PrinterCmdUtils.nextLine(2));
            SendData2USB("请妥善保存你的运输单。 \r\n");
            SendData2USB($"客服电话:{transportReceipt.CustomerServicePhone} \r\n");
            SendData2USB($"公司网址:{transportReceipt.CompanyWebSite} \r\n");
            SendData2USB($"发站地址:{transportReceipt.StartStationAddr} \r\n");
            SendData2USB($"到站地址:{transportReceipt.TerminusAddr} \r\n");
        }

        private void PrintSpecialState()
        {
            var strArr = transportReceipt.RenderImportant_info(transportReceipt.ImportantInfo);
            if (strArr.Length > 0)
            {
                foreach (var str in strArr)
                {
                    SendData2USB(str +" \r\n");
                }
            }
        }

        private void PrintTitleSp()
        {
            SendData2USB(new byte[]
            {
                Core.Printer.PrinterCmdUtils.SP, Core.Printer.PrinterCmdUtils.SP,
                Core.Printer.PrinterCmdUtils.SP, Core.Printer.PrinterCmdUtils.SP,
                Core.Printer.PrinterCmdUtils.SP, Core.Printer.PrinterCmdUtils.SP,
                Core.Printer.PrinterCmdUtils.SP, Core.Printer.PrinterCmdUtils.SP
            });
        }
    }


}
