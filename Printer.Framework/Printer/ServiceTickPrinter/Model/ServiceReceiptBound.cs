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
        public int Total {
            get
            {
                if (Orders == null)
                    return 0;
                return Orders.Count;
            }
        }
        public string Way { get; set; }
    }

    public class Order
    {
        public string No { get; set; }
        public string Reciever { get; set; }
        public string RecieverPhone { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public int PassCount { get; set; }
    }
}
