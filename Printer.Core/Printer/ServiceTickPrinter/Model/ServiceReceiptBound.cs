using System;
using System.Collections.Generic;

namespace Printer.Core.Printer.ServiceTickPrinter.Model
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

        public string NoBytes => StringHelper.GetAdjustedString(No, (string) "   货物编号   ");
        public string Reciever { get; set; }
        public string RecieverBytes=> StringHelper.GetAdjustedString(Reciever, (string) "  收货人  ");
        public string RecieverPhone { get; set; }
        public string RecieverPhoneBytes => StringHelper.GetAdjustedString(RecieverPhone, (string) " 收货人电话 ");
        public string Type { get; set; }
        public string TypeBytes => StringHelper.GetAdjustedString(Type, (string) "   货物编号   ");
        /// <summary>
        /// 提
        /// </summary>
        public int TakePay { get; set; }

        public string TakePayBytes
        {
            get
            {
                var item = $"提:{TakePay}";
                return StringHelper.GetAdjustedString(item, (string) "  收货人  ");
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
                return StringHelper.GetAdjustedString(item, (string) " 收货人电话 ");
            }
        }
    }
}
