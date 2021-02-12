using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_APP.Data.Tables
{
    public class Company
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyId { get; set; }
        public string ShopName { get; set; }
        public string ContactNo { get; set; }
        public string ShopAddress { get; set; }
        public string PrinterName { get; set; }
        public string ImageUrl { get; set; }
    }
}
