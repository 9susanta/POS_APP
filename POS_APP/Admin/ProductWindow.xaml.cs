using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using POS_APP.Client;
using POS_APP.Data.Tables;
using POS_APP.DataLayer;
using POS_APP.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace POS_APP.Admin
{
    /// <summary>
    /// Interaction logic for ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : MetroWindow
    {
        private int _productId = 0;

        List<Products> listProduct;

        int currentPage = 0;
        public ProductWindow()
        {
            InitializeComponent();
            listProduct = new List<Products>();
        }
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            BindProducts();
            BindCategory();
            
        }

        private void BindProducts()
        {
            Task.Factory.StartNew(() =>

                  listProduct = (List<Products>)new DataAccessLayer().GetProducts().Result
           ).ContinueWith(x => BindProducts(currentPage));
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var settings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "Cancel",
                    AnimateShow = true,
                    AnimateHide = false
                };
                var result = await this.ShowMessageAsync("Confirmation", "Are you sure to Delete?", MessageDialogStyle.AffirmativeAndNegative, settings);

                if (result == MessageDialogResult.Affirmative)
                {
                    Products obj = ((FrameworkElement)sender).DataContext as Products;
                    if (obj != null)
                    {
                        DataAccessLayer DAL = new DataAccessLayer();
                        int i = DAL.RemoveProduct(obj.ProductsId);
                        if (i > 0)
                        {
                            await this.ShowMessageAsync("Product", "Product Deleted Sucessfully !");

                            //BindProducts();
                            // BuildItemCode();

                        }
                        else
                        {
                            await this.ShowMessageAsync("Product", "Some Thing Went Wrong !");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Products obj = ((FrameworkElement)sender).DataContext as Products;
            txtCode.Text = obj.ProdCode;
            txtName.Text = obj.ProdName;
            txtPrice.Text = obj.Rates.ToString();
            txtTax.Text = obj.Tax.ToString();
            cmbCategoryName.SelectedValue = obj.Category.Id;
            _productId = obj.ProductsId;
            btnAdd.Content = "Update";

        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtCode.Text=="")
                {
                    await this.ShowMessageAsync("Item", "Please Enter Text Code !");
                    return;
                }
                if (cmbCategoryName.SelectedIndex == -1 || cmbCategoryName.SelectedIndex == 0)
                {
                    await this.ShowMessageAsync("Item", "Please Select a Category !");
                    return;
                }
                if (txtName.Text == "")
                {
                    txtName.Focus();
                    await this.ShowMessageAsync("Item", "Please Enter Item Name !");
                    return;
                }
                if (txtPrice.Text == "")
                {
                    txtPrice.Focus();
                    await this.ShowMessageAsync("Item", "Please Enter Price !");
                    return;
                }
                decimal _lvTax = 0;
                if (txtTax.Text != "")
                {
                    _lvTax = decimal.Parse(txtTax.Text);
                }
                if (btnAdd.Content.ToString() == "Add")
                {
                    DataAccessLayer DAL = new DataAccessLayer();
                    Category lvCat = DAL.GetOneCategory(((Category)cmbCategoryName.SelectedItem).Id).Result;
                    Products products = new Products();
                    products.ProdCode = txtCode.Text.ToUpper();
                    products.ProdName = txtName.Text.ToUpper();
                    products.CategoryId = lvCat.Id;
                    products.Rates = decimal.Parse(txtPrice.Text);
                    products.Tax = _lvTax;
                    products.IsDeleted = false;
                    int i = DAL.AddProduct(products);
                    if (i > 0)
                    {
                        ClearScreen();
                        await this.ShowMessageAsync("Item", "Item Added Sucessfully !");
                       // BuildItemCode();
                        BindProducts();
                    }
                    else if (i == 0)
                    {
                        await this.ShowMessageAsync("Item", "Item Already Exists !");
                    }
                    else
                    {
                        await this.ShowMessageAsync("Item", "Something Went Wrong !");
                    }
                }
                else
                {
                    DataAccessLayer DAL = new DataAccessLayer();
                    Category lvCat = DAL.GetOneCategory(((Category)cmbCategoryName.SelectedItem).Id).Result;
                    Products products = DAL.GetOneProduct(_productId).Result;
                    products.ProdCode = txtCode.Text;
                    products.ProdName = txtName.Text.ToUpper();
                    products.CategoryId = lvCat.Id;
                    products.Rates = decimal.Parse(txtPrice.Text);
                    products.Tax = _lvTax;
                    products.IsDeleted = false;
                    int i = DAL.UpdateProducts(products);
                    if (i > 0)
                    {
                        ClearScreen();
                        await this.ShowMessageAsync("Item", "Item Updated Sucessfully !");
                        //BuildItemCode();
                        BindProducts();
                    }
                    else if (i == 0)
                    {
                        await this.ShowMessageAsync("Item", "Sorry, Same Item Already Exists, Please Choose Diffrent Item Name !");
                    }
                    else
                    {
                        await this.ShowMessageAsync("Item", "Something Went Wrong !");
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void ClearScreen()
        {
            txtCode.Text = "";
            txtName.Text = "";
            txtPrice.Text = "";
            txtTax.Text = "";
            cmbCategoryName.SelectedIndex = 0;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearScreen();
            //BuildItemCode();

        }
        private void BindCategory()
        {
            try
            {
                Task.Factory.StartNew(() =>
              Dispatcher.BeginInvoke(new Action(() =>
              {
                  List<Category> listCategories = (List<Category>)new DataAccessLayer().GetCategories().Result;
                  listCategories.Insert(0, new Category { Id = 0, Name = "--Select a Category--" });

                  cmbCategoryName.ItemsSource = listCategories;
                  cmbCategoryName.SelectedValuePath = "Id";
                  cmbCategoryName.DisplayMemberPath = "Name";

              }), DispatcherPriority.Background)
              );
                

            }
            catch (Exception ex)
            {

            }
        }
        private void BuildItemCode()
        {
            try
            {
                Task.Factory.StartNew(() =>
               Dispatcher.BeginInvoke(new Action(() =>
               {
                   DataAccessLayer DAL = new DataAccessLayer();
                   var lastItem = DAL.GetLastProductData().Result;
                   if (lastItem != null)
                   {
                       txtCode.Text = "MRD 0" + (lastItem.ProductsId + 1);
                   }
                   else
                   {
                       txtCode.Text = "MRD 01";
                   }

               }), DispatcherPriority.Background)
               );
            }
            catch (Exception)
            {
            }
        }

        private void BindProducts(int pageNo)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    Paging PagedTbl = new Paging();
                    PagedTbl.PageIndex = pageNo;
                    gridCategory.ItemsSource = PagedTbl.SetPaging<Products>(listProduct,10);

                }), DispatcherPriority.Background);

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void txtPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
            if (regex.IsMatch(e.Text) && !(e.Text == "." && ((TextBox)sender).Text.Contains(e.Text)))
                e.Handled = false;

            else
                e.Handled = true;
        }

        private void txtTax_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
            if (regex.IsMatch(e.Text) && !(e.Text == "." && ((TextBox)sender).Text.Contains(e.Text)))
                e.Handled = false;

            else
                e.Handled = true;
        }
        private void mnPos_Click(object sender, RoutedEventArgs e)
        {
            PosWindow pos = new PosWindow();
            pos.Show();
            this.Close();
        }

        private void mnCategory_Click(object sender, RoutedEventArgs e)
        {
            CategoryScreen cat = new CategoryScreen();
            cat.Show();
            this.Close();
        }

        private void mnSetting_Click(object sender, RoutedEventArgs e)
        {
            Settings cat = new Settings();
            cat.Show();
            this.Close();
        }

        private async void Backwards_Click(object sender, RoutedEventArgs e)
        {
            currentPage--;
            if (currentPage < 0)
            {
                await this.ShowMessageAsync("Item", "No More Records Present !");
            }
            BindProducts(currentPage);
        }

        private void First_Click(object sender, RoutedEventArgs e)
        {
            BindProducts(0);
        }

        private void Last_Click(object sender, RoutedEventArgs e)
        {
            currentPage = listProduct.Count / 10;
            BindProducts(currentPage);
        }

        private async void Forward_Click(object sender, RoutedEventArgs e)
        {
            currentPage++;
            if(currentPage> (listProduct.Count / 10))
            {
                await this.ShowMessageAsync("Item", "No More Records Present !");
            }
            BindProducts(currentPage);
        }
    }
}
