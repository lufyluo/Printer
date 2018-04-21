using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Printer.Framework.Config;
using Printer.Framework.Printer.Model;
using Printer.Framework.Printer.ServiceTickPrinter.Model;

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
            SendData2USB(PrinterCmdUtils.lineSpace(51));
            SendData2USB(PrinterCmdUtils.nextLine(1));
        }
        /// <summary>
        /// 打印标题
        /// </summary>
        /// <param name="tile"></param>
        protected void PrintTitle(string tile, bool isCenter = true)
        {
            if (isCenter)
            {
                SendData2USB(PrinterCmdUtils.alignCenter());
            }

            SendData2USB(_fontBold);
            SendData2USB(tile + "\r\n");
            SendData2USB(PrinterCmdUtils.reset());
            //SendData2USB(PrinterCmdUtils.nextLine(1));
            if (isCenter) SendData2USB(enddata);
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
            SendData2USB(PrinterCmdUtils.alignCenter());
            SendData2USB(new byte[] { 29, 104, 100 });//h
            SendData2USB(new byte[] { 29, 119, (byte)3.5 });//w
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
                byteArr[i] = PrinterCmdUtils.SP;
            }
            SendData2USB(byteArr);
        }

        private void PrintCommonTableBody(List<GoodsDetail> goodsList)
        {
            foreach (var item in goodsList)
            {
                SendData2USB("┣━━━━━━━━━━╋━━━━━╋━━╋━━┫");
                SendData2USB($@"┃{item.TypeBytes}┃{item.PackageBytes}┃{item.CountBytes}┃{item.TotalBytes}┃");
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

        public static void SetPrinters()
        {
            printerPaths.Clear();
            NewUsb.FindUSBPrinter();
            if (NewUsb.mCurrentDevicePath.Count <= 0)
                return;
            GetPrinterConfigItem("StickPrinter", "Label", NewUsb.mCurrentDevicePath, printerPaths);
            GetPrinterConfigItem("ReceiptPrinter", "Receipt", NewUsb.mCurrentDevicePath, printerPaths);
           
            NewUsb.CloseUSBPort();
        }

        private static void GetPrinterConfigItem(string configKey, string tag, List<string> usbPaths,List<PrinterConfig> printers)
        {
            var printerPort = ConfigManager.GetSetting(configKey);
            if (!string.IsNullOrEmpty(printerPort)&& printerPort!="-1")
            {
                int port = int.Parse(printerPort);
                if (usbPaths.Count - 1 >= port)
                {
                    printers.Add( new PrinterConfig()
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
                var printConfig = printerPaths.FirstOrDefault(n=>n.Tag==tag&&n.Port==int.Parse(port));
                if (printConfig == null)
                    return JsonConvert.SerializeObject(new AciontResult()
                    {
                        Success = false,
                        Desc = "请检查打印机或按P设置打印机！"
                    });
                var result = printConfig.Path == usbPath;
                return JsonConvert.SerializeObject(new AciontResult()
                {
                    Success = result,
                    Desc = result ? "状态正常" : "请检查打印机或按P设置有效打印机！"
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
