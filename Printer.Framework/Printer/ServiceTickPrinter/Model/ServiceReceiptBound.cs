using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printer.Framework.Printer.ServiceTickPrinter.Model
{
    public class ServiceReceiptBound
    {
        public string Title => "门店大票清单";
        public DateTime Date { get; set; }
        public string Store { get; set; }
        public List<Order> Orders { get; set; }
        public string Way { get; set; }
    }

    public class Order
    {
        public string No { get; set; }

        public string NoBytes => No.GetAdjustedString("   货物编号   ");
        public string Reciever { get; set; }
        public string RecieverBytes=> Reciever.GetAdjustedString("  收货人  ");
        public string RecieverPhone { get; set; }
        public string RecieverPhoneBytes => RecieverPhone.GetAdjustedString(" 收货人电话 ");
        public string Type { get; set; }
        public string TypeBytes => Type.GetAdjustedString("   货物编号   ");
        /// <summary>
        /// 提
        /// </summary>
        public int TakePay { get; set; }

        public string TakePayBytes
        {
            get
            {
                var item = $"提:{TakePay}";
                return item.GetAdjustedString("  收货人  ");
            }
        }
        /// <summary>
        /// 代
        /// </summary>
        public string Collection { get; set; }
        public string CollectionBytes
        {
            get
            {
                var item = $"代:{Collection}";
                return item.GetAdjustedString(" 收货人电话 ");
            }
        }
    }
}
