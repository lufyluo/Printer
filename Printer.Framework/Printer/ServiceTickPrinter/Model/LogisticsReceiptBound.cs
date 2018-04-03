using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printer.Framework.Printer.ServiceTickPrinter.Model
{
    public class LogisticsReceiptBound:TransportReceiptBound
    {
        public double Pay { get; set; }
        public double PayBack { get; set; }
        /// <summary>
        /// 到付应收
        /// </summary>
        public double FinalPay { get; set; }
    }
}
