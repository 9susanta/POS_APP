using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_APP.Data.Tables
{
    public class SellRecords
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SellRecordsId { get; set; }
        public string InvoiceNo { get; set; }
        public decimal Amount { get; set; }
        public DateTime PurchaseDate { get; set; }
        public ICollection<InvoiceProduct> InvoiceProduct { get; set; }
    }
}
