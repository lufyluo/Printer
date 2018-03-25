using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Printer.Framework.Printer.ServiceTickPrinter.Model;

namespace Printer.Framework.Printer.ServiceTickPrinter
{
    public class PrinterServiceTick
    {
        public string Test;
        private readonly string _printer = "StickPrinter";
        public string getTest()
        {
            return Test;
        }
        public void setTest(string value)
        {
             Test= value;
        }

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
        public void print()
        {
            Externs.SetDefaultPrinter(_printer);
        }

        //public static EscCommand serviceTickList(Collection<String, Object> resultHashMap)
        //{
        //    EscCommand esc = new EscCommand();
        //    esc.addInitializePrinter();

        //    String goods_quantity_list = MapUtils.toString(resultHashMap, "goods_quantity_list");
        //    ArrayList<HashMap<String, Object>> arrayList = JsonToMapUtils.getList(goods_quantity_list);
        //    if (arrayList == null || arrayList.isEmpty() || arrayList.size() < 1)
        //    {
        //        return null;
        //    }
        //    int count = arrayList.size() / 20 + (arrayList.size() % 20 == 0 ? 0 : 1);
        //    esc.addSetLineSpacing((byte)0);

        //    for (int i = 0; i < count; i++)
        //    {
        //        /* 打印图片 */
        //        esc.addSetLineSpacing((byte)120);
        //        esc.addSelectJustification(EscCommand.JUSTIFICATION.CENTER);
        //        esc.addSetCharcterSize(EscCommand.WIDTH_ZOOM.MUL_2, EscCommand.HEIGHT_ZOOM.MUL_2);
        //        esc.addText("门店大票清单");
        //        esc.addSetCharcterSize(EscCommand.WIDTH_ZOOM.MUL_1, EscCommand.HEIGHT_ZOOM.MUL_1);
        //        esc.addPrintAndLineFeed();

        //        esc.addSetLineSpacing((byte)60);
        //        esc.addSelectJustification(EscCommand.JUSTIFICATION.LEFT);
        //        esc.addText("时间:" + MapUtils.toString(resultHashMap, "print_time"));
        //        esc.addSetHorAndVerMotionUnits((byte)7, (byte)0);
        //        esc.addSetAbsolutePrintPosition((short)12);
        //        esc.addText("当前第" + (i + 1) + "页/共" + count + "页");
        //        esc.addPrintAndLineFeed();

        //        esc.addSelectJustification(EscCommand.JUSTIFICATION.LEFT);
        //        esc.addText("门店:" + MapUtils.toString(resultHashMap, "service_name"));
        //        esc.addSetHorAndVerMotionUnits((byte)7, (byte)0);
        //        esc.addSetAbsolutePrintPosition((short)12);
        //        esc.addText("线路:" + MapUtils.toString(resultHashMap, "route"));
        //        esc.addPrintAndLineFeed();
        //        //列表数据
        //        esc.addSetLineSpacing((byte)0);
        //        esc.addText("┏━┳━━━━━━━━┳━━━━┳━━━━━━┓\r\n");
        //        esc.addText("┃序┃    货物编号    ┃ 收货人 ┃ 收货人电话 ┃\r\n");

        //        int lowIndex = i * 20;
        //        int heiIndex = (i + 1) * 20;
        //        if (heiIndex > arrayList.size())
        //        {
        //            heiIndex = arrayList.size();
        //        }
        //        for (int j = lowIndex; j < heiIndex; j++)
        //        {
        //            esc.addText("┣━╋━━━━━━━━╋━━━━╋━━━━━━┫\r\n");
        //            String index = MapUtils.toString(arrayList.get(j), "index");
        //            String goods_name = MapUtils.toString(arrayList.get(j), "goods_name");
        //            esc.addText("┃" + SubStrUtils.getSubString(index, 2));
        //            esc.addText("┃");
        //            esc.addTurnEmphasizedModeOnOrOff(EscCommand.ENABLE.ON);
        //            esc.addText(SubStrUtils.getSubString(MapUtils.toString(arrayList.get(j), "goods_number"), 16));//显示后12位数据 长度18->16
        //            esc.addTurnEmphasizedModeOnOrOff(EscCommand.ENABLE.OFF);
        //            esc.addText("┃" + SubStrUtils.getSubString(MapUtils.toString(arrayList.get(j), "consignee_name"), 8));
        //            esc.addText("┃" + SubStrUtils.getSubString(MapUtils.toString(arrayList.get(j), "consignee_phone"), 12));
        //            //                esc.addText("┃"+SubStrUtils.getSubString(MapUtils.toString(arrayList.get(j),"goods_name"),4));
        //            esc.addText("┃\r\n");
        //            esc.addText("┃  ┣━━━━━━━━╋━━━━╋━━━━━━┫\r\n");
        //            if (index.getBytes().length > 2)
        //            {
        //                index = index.substring(2, 3);
        //            }
        //            else
        //            {
        //                index = "";
        //            }
        //            esc.addText("┃" + SubStrUtils.getSubString(index, 2));
        //            esc.addText("┃");
        //            esc.addTurnEmphasizedModeOnOrOff(EscCommand.ENABLE.ON);
        //            esc.addText(SubStrUtils.getSubString(MapUtils.toString(arrayList.get(j), "goods_name"), 16));
        //            esc.addTurnEmphasizedModeOnOrOff(EscCommand.ENABLE.OFF);

        //            esc.addText("┃");
        //            esc.addTurnEmphasizedModeOnOrOff(EscCommand.ENABLE.ON);
        //            esc.addText(SubStrUtils.getSubString("提:" + MapUtils.toString(arrayList.get(j), "pay_on_delivery_amount"), 8));//6->8
        //            esc.addTurnEmphasizedModeOnOrOff(EscCommand.ENABLE.OFF);

        //            esc.addText("┃");
        //            esc.addTurnEmphasizedModeOnOrOff(EscCommand.ENABLE.ON);
        //            String is_deduction_freight = MapUtils.toString(arrayList.get(j), "is_deduction_freight");
        //            if (is_deduction_freight == null || is_deduction_freight.isEmpty() || "2".equals(is_deduction_freight))
        //            {
        //                esc.addText(SubStrUtils.getSubString("代:" + SubStrUtils.getDigist(MapUtils.toString(arrayList.get(j), "cash_on_delivery_amount")), 12));
        //            }
        //            else
        //            {
        //                esc.addText(SubStrUtils.getSubString("代:" + SubStrUtils.getDigist(MapUtils.toString(arrayList.get(j), "cash_on_delivery_amount")) + " 扣", 12));
        //            }
        //            esc.addTurnEmphasizedModeOnOrOff(EscCommand.ENABLE.OFF);
        //            //                esc.addText("┃"+SubStrUtils.getSubString("",4));

        //            esc.addText("┃\r\n");
        //        }
        //        esc.addText("┗━┻━━━━━━━━┻━━━━┻━━━━━━┛");

        //        esc.addText("\r\n");
        //        esc.addSetLineSpacing((byte)60);
        //        esc.addPrintAndLineFeed();
        //        esc.addPrintAndLineFeed();
        //        esc.addCutAndFeedPaper((byte)8);

        //    }
    }
}
