using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using POS_APP.Client;
using POS_APP.Data.Tables;
using POS_APP.DataLayer;
using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace POS_APP.Admin
{
    /// <summary>
    /// Interaction logic for CategoryScreen.xaml
    /// </summary>
    public partial class CategoryScreen : MetroWindow
    {
        private int _catId = 0;
        private string _catName = "";
        public CategoryScreen()
        {
            InitializeComponent();
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if(txtCategoryName.Text.Trim()=="")
            {
                await this.ShowMessageAsync("Category", "Please Enter a Category");
                return;
            }
            if (btnAdd.Content.ToString() == "Add")
            {
                DataAccessLayer dataLayer = new DataAccessLayer();
                Category category = new Category();
                category.Name = txtCategoryName.Text.Trim().ToUpper();
                category.IsDeleted = false;
                int i = dataLayer.AddCategory(category);
                if (i > 0)
                {
                    await this.ShowMessageAsync("Category", "Category Saved Sucessfully !");
                    ClearScreen();
                    LoadGrid();
                    
                }
                else if (i == 0)
                {
                    await this.ShowMessageAsync("Category", "Category Already Exists !");
                }
                else
                {
                    await this.ShowMessageAsync("Category", "Something Went Wrong !");
                }
            }
            else
            {
                if(_catName== txtCategoryName.Text.Trim())
                {
                    await this.ShowMessageAsync("Category", "Same Category !");
                    return;
                }
                DataAccessLayer dataLayer = new DataAccessLayer();
                Category category = new Category();
                category.Name = txtCategoryName.Text.Trim().ToUpper();
                category.IsDeleted = false;
                category.Id = _catId;
                int i = dataLayer.UpdateCategory(category);
                if (i > 0)
                {
                    await this.ShowMessageAsync("Category", "Category Updated Sucessfully !");
                    ClearScreen();
                    LoadGrid();
                }
                else if (i == 0)
                {
                    await this.ShowMessageAsync("Category", "Category Already Exists !");
                }
                else
                {
                    await this.ShowMessageAsync("Category", "Something Went Wrong !");
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadGrid();
        }
        private void LoadGrid()
        {
            Task.Factory.StartNew(() =>
            Dispatcher.BeginInvoke(new Action(() =>
            {
                DataAccessLayer dataLayer = new DataAccessLayer();
                gridCategory.ItemsSource = dataLayer.GetCategories().Result;

            }), DispatcherPriority.Background)
            ); 
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                Category obj = ((FrameworkElement)sender).DataContext as Category;
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    DataAccessLayer dataLayer = new DataAccessLayer();
                    dataLayer.DeleteCategory(obj);
                    await this.ShowMessageAsync("Category", "Category Deleted Sucessfully !");
                    LoadGrid();
                    ClearScreen();

                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("", ex.Message);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Category obj = ((FrameworkElement)sender).DataContext as Category;
            if(obj!=null)
            {
                btnAdd.Content = "Edit";
                txtCategoryName.Text = obj.Name;
                _catName = obj.Name;
                _catId = obj.Id;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearScreen();
            
        }
        private void ClearScreen()
        {
            txtCategoryName.Text = "";
            if (btnAdd.Content.ToString() == "Edit")
            {
                btnAdd.Content = "Add";
                _catId = 0;
            }
        }

        private void mnPos_Click(object sender, RoutedEventArgs e)
        {
            PosWindow pos = new PosWindow();
            pos.Show();
            this.Close();
        }

        private void mnProduct_Click(object sender, RoutedEventArgs e)
        {
            ProductWindow cat = new ProductWindow();
            cat.Show();
            this.Close();
        }

        private void mnSetting_Click(object sender, RoutedEventArgs e)
        {
            Settings cat = new Settings();
            cat.Show();
            this.Close();
        }
    }
}
