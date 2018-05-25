﻿using System;
using Newtonsoft.Json;
using Printer.Core.Printer.ServiceTickPrinter.Model;
using Printer.Framework.Config;
using Printer.Framework.Printer;
namespace Printer.Core.Printer.ServiceTickPrinter
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
                    SendData2USB(Core.Printer.PrinterCmdUtils.reset());
                    SendData2USB(Core.Printer.PrinterCmdUtils.printNextLine(1));
                    //LoadPOSDll.POS_SetLineSpacing(100);
                    MainPrint("存根联");
                    SpecialState1();
                    SendData2USB(Core.Printer.PrinterCmdUtils.reset());
                    SendData2USB(Core.Printer.PrinterCmdUtils.nextLine(1));
                    SendData2USB(Core.Printer.PrinterCmdUtils.printNextLine(2));
                    MainPrint("客户联");
                    SpecialState2();
                    SendData2USB(Core.Printer.PrinterCmdUtils.printNextLine(2));
                    SendData2USB(enddata);
                    SendData2USB(" \r\n");
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
                return JsonConvert.SerializeObject(new AciontResult()
                {
                    Success = false,
                    Desc = e.Message
                });
            }

        }

        private void MainPrint(string type)
        {
            SendData2USB(Core.Printer.PrinterCmdUtils.reset());
            PrintTitle(logisticsReceiptBound.Title + $"签收单({type})");
            SendData2USB(Core.Printer.PrinterCmdUtils.setLineHeight(20));
            SendData2USB(Core.Printer.PrinterCmdUtils.printNextLine(1));
            PrintBar(logisticsReceiptBound.BarCode, false);
            SendData2USB(Core.Printer.PrinterCmdUtils.setLineHeight(20));
            SendData2USB(Core.Printer.PrinterCmdUtils.printNextLine(1));
            //SendData2USB(enddata);
            PrintHead();
            PrintBody(type);
        }

        private void PrintHead()
        {
            var maxLengthString = $"托运时间:{logisticsReceiptBound.ConsignmentDate.ToString("yyyy-MM-dd")}";
            var exceptLent = $"托运{logisticsReceiptBound.ConsignmentDate.ToString("yyyy-MM-dd")}";
            PrintTitle($"{logisticsReceiptBound.Terminus}:{logisticsReceiptBound.Goods}");
            SendData2USB(Core.Printer.PrinterCmdUtils.setLineHeight(20));
            SendData2USB(Core.Printer.PrinterCmdUtils.printNextLine(1));
            SendData2USB(Core.Printer.PrinterCmdUtils.resetLineHeight());
            SendData2USB(StringHelper.GetAdjustedString((string)$"发站:{logisticsReceiptBound.StartStation}", maxLengthString));
            PrintTitleSp(2);
            SendData2USB($"电话:{logisticsReceiptBound.Mobile}");
            SendData2USB(enddata);
            SendData2USB($"到站:");
            SendData2USB(Core.Printer.PrinterCmdUtils.setBold(1));
            SendData2USB(StringHelper.GetAdjustedString((string)$"{logisticsReceiptBound.Terminus}", exceptLent));
            SendData2USB(Core.Printer.PrinterCmdUtils.setBold(0));
            PrintTitleSp(2);
            SendData2USB($"电话:");
            SendData2USB(Core.Printer.PrinterCmdUtils.setBold(1));
            SendData2USB($"{logisticsReceiptBound.TelPhone}" + " \r\n");
            SendData2USB(Core.Printer.PrinterCmdUtils.setBold(0));
            //SendData2USB(enddata);
            SendData2USB(maxLengthString);
            PrintTitleSp(2);
            SendData2USB($"开单时间:{logisticsReceiptBound.OperateTime}");
            SendData2USB(enddata);
            SendData2USB($"打印时间:{logisticsReceiptBound.PrintDate}");
            SendData2USB(enddata);
            PrintLine();
        }
        private void PrintBody(string type)
        {
            var maxLengthString = $"托运时间:{logisticsReceiptBound.ConsignmentDate.ToString("yyyy-MM-dd")}";
            PrintTitle($"收货:{logisticsReceiptBound.Reciever} {logisticsReceiptBound.RecieverPhone.ToString()} \r\n", false);
            SendData2USB($"{StringHelper.GetAdjustedString(("发货人:" + logisticsReceiptBound.Sender), maxLengthString)}");
            //PrintTitleSp(2);
            SendData2USB($"电话:{logisticsReceiptBound.SenderPhone}");
            SendData2USB(enddata);
            PrintCommonTable(logisticsReceiptBound.GoodsDetails);
            NextLine();
            PrintLine();

            SendData2USB($"送货费:{StringHelper.GetAdjustedStringByCols(logisticsReceiptBound.DeliveryFee.ToString(), (int)12)}");
            //PrintTitleSp(12);
            SendData2USB($"现付:{StringHelper.GetAdjustedStringByCols(logisticsReceiptBound.Pay.ToString(), (int)12)}");
            //PrintTitleSp(12);
            SendData2USB($"回付:{logisticsReceiptBound.PayBack} \r\n");
            SendData2USB(Core.Printer.PrinterCmdUtils.nextLine(1));
            if (type == "存根联")
            {
                SendData2USB($"备注:{logisticsReceiptBound.BillingStaffRemarks} \r\n");
                SendData2USB(Core.Printer.PrinterCmdUtils.nextLine(1));
            }
            SendData2USB(Core.Printer.PrinterCmdUtils.setLineHeight(25));
            PrintTitle($"提付:{logisticsReceiptBound.TakePay} \r\n", false);
            SendData2USB(Core.Printer.PrinterCmdUtils.setLineHeight(25));
            PrintTitle($"代收款:{logisticsReceiptBound.Collection} \r\n", false);
            PrintTitle($"到付应收:{logisticsReceiptBound.FinalPay}", false);
            if (!string.IsNullOrEmpty(logisticsReceiptBound.BillingStaffRemarks) && type == "客户联")
                PrintTitle($"备注:{logisticsReceiptBound.BillingStaffRemarks} \r\n", false);
        }

        private void SpecialState1()
        {
            PrintLine();
            SendData2USB(Core.Printer.PrinterCmdUtils.printNextLine(2));
            SendData2USB("特别声明:本单有效期一个月\r\n");
            SendData2USB(enddata);
            SendData2USB(" \r\n");
            SendData2USB(Core.Printer.PrinterCmdUtils.nextLine(1));
            SendData2USB("签收人签章:\r\n");
            SendData2USB(" \r\n");
            SendData2USB(enddata);
            SendData2USB(Core.Printer.PrinterCmdUtils.nextLine(1));
            SendData2USB("签收人证件号:\r\n");
            SendData2USB(" \r\n");
            SendData2USB(enddata);
            SendData2USB(Core.Printer.PrinterCmdUtils.nextLine(1));
            PrintCutTipLine();
        }
        private void SpecialState2()
        {
            PrintLine();
            SendData2USB(Core.Printer.PrinterCmdUtils.setBold(1));
            SendData2USB("特别声明:");
            SendData2USB(Core.Printer.PrinterCmdUtils.setBold(0));
            SendData2USB(Core.Printer.PrinterCmdUtils.nextLine(1));

            var strArr = logisticsReceiptBound.RenderImportant_info(logisticsReceiptBound.StatementList);
            if (strArr.Length > 0)
            {
                foreach (var str in strArr)
                {
                    SendData2USB(str + " \r\n");
                }
            }
        }
        private void PrintCutTipLine()
        {
            SendData2USB("-------------------请沿次线撕开-----------------");
            SendData2USB(Core.Printer.PrinterCmdUtils.nextLine(1));
        }
    }
}
