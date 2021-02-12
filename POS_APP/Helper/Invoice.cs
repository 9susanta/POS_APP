using POS_APP.Data.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace POS_APP.Helper
{
    public class Invoice:INotifyPropertyChanged
    {
        public string InvoiceNo{ get; set; }
        public int ProductsId { get; set; }
        public string ProdCode { get; set; }
        public string ProdName { get; set; }
        public string CategoryName { get; set; }
        public decimal Rates { get; set; }
        public decimal Tax { get; set; }

        private int _qty;
        public int Qty { 
            get { return _qty; }
            set
            {
                _qty = value;
                Total = this._qty * this.Rates;
                OnPropertyChanged("Qty");
                OnPropertyChanged("Total");
            }
        }
        public decimal Total { get; set; }
        public decimal SubTotal { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string caller="")
        {
            if(PropertyChanged!=null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }
    }
}
