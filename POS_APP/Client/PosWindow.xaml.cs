using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using POS_APP.Admin;
using POS_APP.Data.Tables;
using POS_APP.DataLayer;
using POS_APP.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace POS_APP.Client
{
    /// <summary>
    /// Interaction logic for PosWindow.xaml
    /// </summary>
    public partial class PosWindow : MetroWindow
    {
        Products _products=null;
        List<Products> lstProduct = new List<Products>();

        private ObservableCollection<Invoice> ShoppingCart;
        public PosWindow()
        {
            InitializeComponent();
            ShoppingCart = new ObservableCollection<Invoice>();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            getQtyNo();
            getCategory();
            getProductByCategory(0);
            BindInvoiceGrid();
            ShoppingCart.CollectionChanged += (s, args) =>
              {
                  if(args.Action==NotifyCollectionChangedAction.Remove)
                  {
                      foreach(Invoice invoice in args.OldItems)
                      {
                          invoice.PropertyChanged -= UpdateTotalSale;
                      }
                  }
                  else if(args.Action == NotifyCollectionChangedAction.Add)
                  {
                      foreach (Invoice invoice in args.NewItems)
                      {
                          invoice.PropertyChanged += UpdateTotalSale;
                      }
                  }
              };
            UpdateSubtotalPrice();
        }

        private void UpdateTotalSale(object sender, PropertyChangedEventArgs e)
        {
            lbtotal.Text = string.Format("Total: {0}", ShoppingCart.Sum(x => x.Total).ToString("C"));
        }

        private void getCategory()
        {
            try
            {
                DataAccessLayer DAL = new DataAccessLayer();
                List<Category> listCategories = (List<Category>)new DataAccessLayer().GetCategories().Result;
                listCategories.Insert(0, new Category { Id = 0, Name = "All Items" });

                listViewCategory.ItemsSource = listCategories;
            }
            catch (Exception ex)
            {

            }
        }
        private void getProductByCategory(int categoryId)
        {
            try
            {
                if (lstProduct.Count == 0)
                {
                    DataAccessLayer DAL = new DataAccessLayer();
                    lstProduct = (List<Products>)DAL.GetProducts().Result;

                }

                if (categoryId == 0)
                {
                    listViewItems.ItemsSource = lstProduct;
                }
                else
                {
                    listViewItems.ItemsSource = lstProduct.Where(x=>x.Category.Id== categoryId).ToList();
                }
            }
            catch (Exception)
            {

            }
        }

        private void btnlistcat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Category obj = ((FrameworkElement)sender).DataContext as Category;
                if (obj != null)
                {
                    getProductByCategory(obj.Id);
                }
            }
            catch (Exception)
            {

            }
        }

        private void btnlistProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Products obj = ((FrameworkElement)sender).DataContext as Products;
                if (obj != null)
                {
                    _products = obj;

                    var shCart=ShoppingCart.FirstOrDefault(s => s.ProductsId == _products.ProductsId);
                    if(shCart!=null)
                    {
                        shCart.Qty = shCart.Qty + 1;
                    }
                    else
                    {
                        ShoppingCart.Add(new Invoice()
                        {
                            ProductsId = _products.ProductsId,
                            ProdCode = _products.ProdCode,
                            ProdName = _products.ProdName,
                            Rates = _products.Rates,
                            Qty = 1,
                            Total = _products.Rates * 1
                        });
                    }
                    UpdateSubtotalPrice();
                    //((Button)sender).Background= Brushes.Blue;
                }
            }
            catch (Exception)
            {
            }
        }
        private void mnCategory_Click(object sender, RoutedEventArgs e)
        {
            CategoryScreen cat = new CategoryScreen();
            cat.Show();
            this.Close();
        }

        private void mnProduct_Click(object sender, RoutedEventArgs e)
        {
            ProductWindow cat = new ProductWindow();
            cat.Show();
            this.Close();
        }

        private void txtSeach_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if(txtSeach.Text.Trim()!=""&&txtSeach.Text.Length>1)
                {
                    if(lstProduct.Count>0)
                    {
                        List<Products> listProd = lstProduct.
                            Where(x => x.ProdName.ToUpper().Contains(txtSeach.Text.ToUpper()) || x.ProdCode.ToUpper().Contains(txtSeach.Text.ToUpper())).ToList();
                        if(listProd.Count>0)
                        {
                            listViewItems.ItemsSource = listProd;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               
            }
        }
        public void getQtyNo()
        {
            try
            {
                BuildQty qty = new BuildQty();
                listViewNo.ItemsSource =qty.getBuildQty();
               
            }
            catch (Exception)
            {

            }
        }

        private async void btnlistNo_Click(object sender, RoutedEventArgs e)
        {
            if (_products==null)
            {
                await this.ShowMessageAsync("POS","Please Select a Item First !!");
                return;
            }
            CountItem count = ((FrameworkElement)sender).DataContext as CountItem;
            if (count != null)
            {
                BuildInvoice(count.ItemNo);
            }
        }

        public void BuildInvoice(int qty)
        {
            if(_products!=null)
            {
                    qty = qty == 0 ? 1 : qty;
                    //we check if product is not already in the cart if it is we remove the old one
                    ShoppingCart.RemoveAll(s => s.ProductsId == _products.ProductsId);
                    //we add the product to the Cart
                    ShoppingCart.Add(new Invoice()
                    {
                        ProductsId = _products.ProductsId,
                        ProdCode = _products.ProdCode,
                        ProdName=_products.ProdName,
                        Rates = _products.Rates,
                        Qty = qty,
                        Total=_products.Rates*qty
                    });
                UpdateSubtotalPrice();
            }
        }

        private void UpdateSubtotalPrice()
        {
            UpdateTotalSale(null, null);
            _products = null;
        }

        public void BindInvoiceGrid()
        {
            dgInvoice.ItemsSource = ShoppingCart;
            dgInvoice.Items.Refresh();
        }

        private void btnInvoice_Click(object sender, RoutedEventArgs e)
        {
            Invoice _invoice = ((FrameworkElement)sender).DataContext as Invoice;
            if (_invoice != null)
            {
                ShoppingCart.RemoveAll(s => s.ProductsId == _invoice.ProductsId);

                UpdateSubtotalPrice();
            }
        }

        private void CancelBilling()
        {
            ShoppingCart.Clear();
            UpdateSubtotalPrice();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CancelBilling();
        }
    }
}
