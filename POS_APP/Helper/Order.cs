using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_APP.Helper
{
    public class Order
    {
        public List<Invoice> lstInvoice { get; set; }
        public string Invoice { get; set; }
        public string SubTotal { get; set; }
        public string OrderDate { get; set; }
        public string CustomerName { get; set; }
    }
}
