using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printer.Framework.Printer.ServiceTickPrinter.Model
{
    public class TransportReceiptBound
    {
        public string Title { get; set; }
        public string BarCode { get; set; }
        public string Goods { get; set; }
        public string StartStation { get; set; }
        public string Mobile { get; set; }
        /// <summary>
        /// 固定电话
        /// </summary>
        public string TelPhone { get; set; }
        /// <summary>
        /// 到站
        /// </summary>
        public string Terminus { get; set; }
        /// <summary>
        /// 托运时间
        /// </summary>
        public DateTime ConsignmentDate { get; set; }
        public string OperateTime { get; set; }
        public string PrintDate { get; set; }
        public string Reciever { get; set; }
        public string RecieverPhone { get; set; }
        public string Sender { get; set; }
        public string SenderPhone { get; set; }
        public List<GoodsDetail > GoodsDetails { get; set; }
        /// <summary>
        /// 运费
        /// </summary>
        public double FreightFee { get; set; }
        /// <summary>
        /// 送货费
        /// </summary>
        public double DeliveryFee { get; set; }
        /// <summary>
        /// 接货费
        /// </summary>
        public double RecieveFee { get; set; }
        /// <summary>
        /// 保险费
        /// </summary>
        public double SecureFee { get; set; }
        public bool IsProtected { get; set; }
        public double TotalFee => TakePay;
        /// <summary>
        /// 提付
        /// </summary>
        public double TakePay { get; set; }
        /// <summary>
        /// 代收款
        /// </summary>
        public string Collection { get; set; }
        public string BillingStaff { get; set; }
        public string VerificationCode { get; set; }
        public string BillingStaffRemarks { get; set; }
        public string CustomerServicePhone { get; set; }
        public string CompanyWebSite { get; set; }
        public string StartStationAddr { get; set; }
        public string TerminusAddr { get; set; }
        public string ShipperBankName { get; set; }
        public string ShipperBankCardAccount { get; set; }
        public int RebateFee { get; set; }
        public string ImportantInfo { get; set; }

        public string PrintCheckCode { get; set; }
        /// <summary>
        /// 大写共计
        /// </summary>
        public string AmountInWords { get; set; }

        public bool IsNeedShowFreightFeeEtc { get; set; }

        public string[] RenderImportant_info(string info)
        {
            if (string.IsNullOrEmpty(info))
            {
                return new string[] { };
            }
            info.Replace("<br>","");
            return info.Split(new string[] { "<br>" }, StringSplitOptions.None);
        }
        public string PrintTime { get; set; }
    }

    public class GoodsDetail
    {
        public string Type { get; set; }
        public string TypeBytes => Type.GetAdjustedString("        品名        ");
        public string Package { get; set; }
        public string PackageBytes => Package.GetAdjustedString("   包装   ");
        public int Count { get; set; }
        public string CountBytes => Count.ToString().GetAdjustedString("件数");
        public int Total { get; set; }
        public string TotalBytes => Total.ToString().GetAdjustedString("总数");
    }
}
