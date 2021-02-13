using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_APP.Data.Tables
{
    public class InvoiceProduct
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Guid InvoiceProductId { get; set; }
        public int ProductIds { get; set; }
        public SellRecords SellRecords { get; set; }
        public int SellRecordsId { get; set; }
    }
}
