using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;

namespace Printer
{
    class Printer
    {
        public void Print()
        {
            //打印预览           

            PrintPreviewDialog ppd = new PrintPreviewDialog();

            PrintDocument pd = new PrintDocument();



            //设置边距

            Margins margin = new Margins(20, 20, 20, 20);

            pd.DefaultPageSettings.Margins = margin;



            ////纸张设置默认

            PaperSize pageSize = new PaperSize("First custom size", getYc(58), 600);

            pd.DefaultPageSettings.PaperSize = pageSize;



            //打印事件设置           

            pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);

            ppd.Document = pd;

            ppd.ShowDialog();



            try

            {

                pd.Print();

            }

            catch (Exception ex)

            {

                MessageBox.Show(ex.Message, "打印出错", MessageBoxButtons.OK);

                pd.PrintController.OnEndPrint(pd, new PrintEventArgs());

            }
        }
        private void pd_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            SetInvoiceData(e.Graphics);
        }

        private void SetInvoiceData(Graphics g)
        {
            Font InvoiceFont = new Font("Arial", 10, FontStyle.Bold);
            SolidBrush GrayBrush = new SolidBrush(Color.Gray);
            g.DrawString(GetPrintStr(), InvoiceFont, GrayBrush, 10, 10);
            g.Dispose();
        }
        private int getYc(double cm)

        {

            return (int)(cm / 25.4) * 100;

        }



        public string GetPrintStr()

        {

            StringBuilder sb = new StringBuilder();



            string tou = "伊尹餐饮公司";

            string address = "深圳市罗湖区东门老街29号";

            string saleID = "2010930233330";

            string item = "项目";

            decimal price = 25.00M;

            int count = 5;

            decimal total = 0.00M;

            decimal fukuan = 500.00M;



            sb.Append("            " + tou + "     /n");

            sb.Append("-----------------------------------------------------------------/n");

            sb.Append("日期:" + DateTime.Now.ToShortDateString() + "  " + "单号:" + saleID + "/n");

            sb.Append("-----------------------------------------------------------------/n");

            sb.Append("项目" + "/t/t" + "数量" + "/t" + "单价" + "/t" + "小计" + "/n");

            for (int i = 0; i < count; i++)

            {

                decimal xiaoji = (i + 1) * price;

                sb.Append(item + (i + 1) + "/t/t" + (i + 1) + "/t" + price + "/t" + xiaoji);

                total += xiaoji;



                if (i != (count))

                    sb.Append("/n");

            }



            sb.Append("-----------------------------------------------------------------/n");

            sb.Append("数量: " + count + " 合计:   " + total + "/n");

            sb.Append("付款: 现金" + "    " + fukuan);

            sb.Append("         现金找零:" + "   " + (fukuan - total) + "/n");

            sb.Append("-----------------------------------------------------------------/n");

            sb.Append("地址：" + address + "/n");

            sb.Append("电话：123456789   123456789/n");



            sb.Append("                 谢谢惠顾欢迎下次光临                    ");

            return sb.ToString();

        }
    }
}
