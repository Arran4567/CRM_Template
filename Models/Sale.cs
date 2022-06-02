using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bicks.Models
{
    public class Sale
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }
        [Required]
        [Display(Name = "Time Of Sale")]
        public string SaleDateTime { get; set; }
        [ForeignKey("SaleInvoiceItems")]
        [Display(Name = "Invoice Items")]
        public virtual List<InvoiceItem> SaleInvoiceItems { get; set; }
        [ForeignKey("ClientId")]
        [Display(Name = "Client")]
        public virtual Client Client { get; set; }

    }
}
