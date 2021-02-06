using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_APP.Data.Tables
{
    public class Products
    {
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductsId { get; set; }
        public string ProdCode { get; set; }
        public string ProdName { get; set; }
        public decimal Rates { get; set; }
        public decimal Tax { get; set; }
        public bool IsDeleted { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
    }
}
