﻿using System;

namespace Printer.Core.Printer.ServiceTickPrinter.Model
{
    public class QrReceiptBound
    {
        public string Type { get; set; }
        public string Terminus { get; set; }
        public string Reciever { get; set; }
        public string Goods { get; set; }
        public string Company { get; set; }
        public string Code { get; set; }
        public Guid TempFileName { get; set; }
        public int Times { get; set; }

        public QrReceiptBound()
        {
            TempFileName = Guid.NewGuid();
        }
    }
}
