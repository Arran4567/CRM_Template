using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bicks.Models;

namespace Bicks.Areas.Invoicing.ViewModels
{
    public class SalesInvoiceViewModel
    {
        public int InvoiceNo { get; set; }
        public DateTime Date { get; set; }
        public string InvoiceTo { get; set; }
        public string DeliverTo { get; set; }
        public List<InvoiceItem> InvoiceItems { get; set; }
        public decimal Total { get; set; }
    }
}
