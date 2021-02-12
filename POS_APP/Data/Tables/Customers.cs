using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_APP.Data.Tables
{
    public class Customers
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string CName { get; set; }
        public string CPhone { get; set; }
        public DateTime PurchaseDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
