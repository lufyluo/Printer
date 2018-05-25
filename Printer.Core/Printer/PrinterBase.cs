using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Printer.Core.Printer.Model;
using Printer.Core.Printer.ServiceTickPrinter.Model;
using Printer.Framework.Config;

namespace Printer.Framework.Printer
{
    public abstract class PrinterBase
    {
        protected static readonly libUsbContorl.UsbOperation NewUsb = new libUsbContorl.UsbOperation();
        public static List<PrinterConfig> printerPaths = new List<PrinterConfig>();
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
            SendData2USB(Core.Printer.PrinterCmdUtils.lineSpace(51));
            SendData2USB(Core.Printer.PrinterCmdUtils.nextLine(1));
        }
        /// <summary>
        /// 打印标题
        /// </summary>
        /// <param name="tile"></param>
        protected void PrintTitle(string tile, bool isCenter = true)
        {
            if (isCenter)
            {
                SendData2USB(Core.Printer.PrinterCmdUtils.alignCenter());
            }
            SendData2USB(Core.Printer.PrinterCmdUtils.setBold(1));
            SendData2USB(_fontBold);
            SendData2USB(tile);
            SendData2USB(Core.Printer.PrinterCmdUtils.printNextLine(1));
            SendData2USB(Core.Printer.PrinterCmdUtils.reset());
            //SendData2USB(PrinterCmdUtils.nextLine(1));
            //if (isCenter) SendData2USB(enddata);
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
        protected void PrintBar(string code, bool isNeedCodeShown = true)
        {
            byte barCodeLength = (byte)(code.Length+2);
            SendData2USB(Core.Printer.PrinterCmdUtils.alignCenter());
            SendData2USB(new byte[] { 29, 104, 100 });//h
            SendData2USB(new byte[] { 29, 119, 2});//w
            if (isNeedCodeShown)
                SendData2USB(new byte[] { 29, 72, 50 });
            SendData2USB(new byte[] { 29, 107 });
            SendData2USB(new byte[] { 73, barCodeLength, 123, 66 } );
            SendData2USB(code);
        }
        protected void PrintTitleSp(int n)
        {
            var byteArr = new byte[n];
            for (int i = 0; i < byteArr.Length; i++)
            {
                byteArr[i] = Core.Printer.PrinterCmdUtils.SP;
            }
            SendData2USB(byteArr);
        }

        private void PrintCommonTableBody(List<GoodsDetail> goodsList)
        {
            var len = goodsList.Count;
            var index = 0;
            foreach (var item in goodsList)
            {
                SendData2USB("┣━━━━━━━━━━╋━━━━━╋━━╋━━┫");
                var total = index == 0 ? item.TotalBytes : "";
                SendData2USB($@"┃{item.TypeBytes}┃{item.PackageBytes}┃{item.CountBytes}┃{total}┃");
            }
        }

        public static List<SelectItem> GetAllPrinters()
        {
            SetPrinters();
            NewUsb.FindUSBPrinter();
            List<SelectItem> names = new List<SelectItem>();
            var index = 0;
            names.Add(new SelectItem()
            {
                Id = -1,
                Value = "无"
            });
            foreach (string sPrint in NewUsb.mCurrentDevicePath)//获取所有打印机名称
            {

                var Item = new SelectItem()
                {
                    Id = index,
                    Value = sPrint
                };
                index++;
                names.Add(Item);
            }
            NewUsb.CloseUSBPort();
            return names;
        }
        //public static List<SelectItem> GetAllPrinters()
        //{
        //    SetPrinters();
        //    List<SelectItem> names = new List<SelectItem>();
        //    var index = 0;
        //    names.Add(new SelectItem()
        //    {
        //        Id = -1,
        //        Value = "无"
        //    });
        //    names.Add(new SelectItem()
        //    {
        //        Id = 0,
        //        Value = "this is 0"
        //    });
        //    names.Add(new SelectItem()
        //    {
        //        Id = 1,
        //        Value = "this is 1"
        //    });
        //    names.Add(new SelectItem()
        //    {
        //        Id = 2,
        //        Value = "this is 2"
        //    });
        //    return names;
        //}
        //public static void SetPrinters()
        //{
        //    var names = new List<SelectItem>();
        //    var index = 0;
        //    names.Add(new SelectItem()
        //    {
        //        Id = 0,
        //        Value = "this is 0"
        //    });
        //    names.Add(new SelectItem()
        //    {
        //        Id = 1,
        //        Value = "this is 1"
        //    });
        //    names.Add(new SelectItem()
        //    {
        //        Id = 2,
        //        Value = "this is 2"
        //    });
        //    printerPaths.Clear();
        //    foreach (var name in names)
        //    {
        //        GetPrinterConfigItem("StickPrinter", "UnKnown", names.Select(n => n.Value).ToList());
        //    }
        //    //GetPrinterConfigItem("StickPrinter", "Label", names.Select(n=>n.Value).ToList());
        //    //GetPrinterConfigItem("ReceiptPrinter", "Receipt", names.Select(n => n.Value).ToList());
        //}
        public static void SetPrinters()
        {
            printerPaths.Clear();
            NewUsb.FindUSBPrinter();
            if (NewUsb.mCurrentDevicePath.Count <= 0)
                return;
            foreach (var name in NewUsb.mCurrentDevicePath)
            {
                GetPrinterConfigItem("StickPrinter", "UnKnown", NewUsb.mCurrentDevicePath);
            }
            NewUsb.CloseUSBPort();
        }
        private static void GetPrinterConfigItem(string configKey, string tag, List<string> usbPaths)
        {
            var printerPort = ConfigManager.GetSetting(configKey);
            if (!string.IsNullOrEmpty(printerPort)&& printerPort!="-1")
            {
                int port = int.Parse(printerPort);
                if (usbPaths.Count > port)
                {
                    printerPaths.Add( new PrinterConfig()
                    {
                        Port = port,
                        Path = usbPaths[port],
                        Tag = tag
                    });
                }

            }
        }

        /// <summary>
        /// js Interface
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public string isPrinterOk(string tag)
        {
            try
            {
                if (string.IsNullOrEmpty(tag))
                    return JsonConvert.SerializeObject(new AciontResult()
                    {
                        Success = false,
                        Desc = "参数错误！"
                    });
                var key = tag == "Label" ? "StickPrinter" : "ReceiptPrinter";
                var port = ConfigManager.GetSetting(key);
                if (string.IsNullOrEmpty(port))
                    return JsonConvert.SerializeObject(new AciontResult()
                    {
                        Success = false,
                        Desc = "请检查打印机或按P设置有效打印机！"
                    });
                var usbPath = ConfigManager.GetSetting(key + "Path");
                if (string.IsNullOrEmpty(usbPath))
                    return JsonConvert.SerializeObject(new AciontResult()
                    {
                        Success = false,
                        Desc = "请按P重新设置打印机！"
                    });


                SetPrinters();
                if (!printerPaths.Any())
                    return JsonConvert.SerializeObject(new AciontResult()
                    {
                        Success = false,
                        Desc = "未检测到打印机！"
                    });
                return JsonConvert.SerializeObject(new AciontResult()
                {
                    Success = true,
                    Desc =  "打印机已配置！"
                });
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(new AciontResult()
                {
                    Success = false,
                    Desc = "请检查打印机或按P设置有效打印机！"
                });
            }
            
        }
    }
}
