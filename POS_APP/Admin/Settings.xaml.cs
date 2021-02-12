using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using POS_APP.Client;
using POS_APP.Data.Tables;
using POS_APP.DataLayer;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
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

namespace POS_APP.Admin
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : MetroWindow
    {
        public string logoUrl = "";
        public Settings()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            getAppPrinter();
            BindCompanyData();
        }

        private async void btnUpdCompany_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtShopName.Text.Trim() == "")
                {
                    await this.ShowMessageAsync("Settings", "Shop Name Cannot be empty !");
                    return;
                }
                if (txtContact.Text.Trim() == "")
                {
                    await this.ShowMessageAsync("Settings", "Contact No Cannot be empty !");
                    return;
                }
                if (txtAddress.Text.Trim() == "")
                {
                    await this.ShowMessageAsync("Settings", "Shop Address Cannot be empty !");
                    return;
                }

                DataAccessLayer dataAcessLayer = new DataAccessLayer();
                Company company = new Company();
                company.ShopName = txtShopName.Text;
                company.ContactNo = txtContact.Text;
                company.ShopAddress = txtAddress.Text;
                company.ImageUrl = logoImg.Source.ToString().Replace("file:///","");
                int i= dataAcessLayer.UpdateCompanies(company);
                if(i>0)
                {
                    await this.ShowMessageAsync("Settings", "Shop Details Updated Sucessfully !");
                }
                BindCompanyData();
            }
            catch (Exception ex)
            {

                await this.ShowMessageAsync("Settings", ex.Message);
            }
        }

        private async void btnPrinterUdt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataAccessLayer dataAcessLayer = new DataAccessLayer();
                Company company = new Company();
                company.PrinterName = cmbPrinter.Text;
                int i=dataAcessLayer.UpdatePrinter(company);
                if(i>0)
                {
                    await this.ShowMessageAsync("Settings", "Printer Set Sucessfully !");
                }
            }
            catch (Exception ex)
            {
              await this.ShowMessageAsync("Settings", ex.Message);
            }
        }
        private void getAppPrinter()
        {
            try
            {
                String lkInstalledPrinters;
                for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
                {
                    lkInstalledPrinters = PrinterSettings.InstalledPrinters[i];
                    cmbPrinter.Items.Add(lkInstalledPrinters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BindCompanyData()
        {
            try
            {
                DataAccessLayer dataAcessLayer = new DataAccessLayer();
                Company company = dataAcessLayer.GetCompany();
                if (company != null)
                {
                    txtShopName.Text = company.ShopName;
                    txtContact.Text = company.ContactNo;
                    txtAddress.Text = company.ShopAddress;
                    cmbPrinter.SelectedValue = company.PrinterName;
                    try
                    {
                        logoImg.Source = new BitmapImage(new Uri(company.ImageUrl));
                    }
                    catch (Exception ex)
                    {
                    }
                    SettPrinter.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {

                
            }
        }
        public void LogoUpload()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
           
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                var image = new BitmapImage(new Uri(op.FileName));
                var fullPath = op.FileName;
                string[] partsFileName = fullPath.Split('\\');
                var applicationPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                var dir = new System.IO.DirectoryInfo(System.IO.Path.Combine(applicationPath, "Asset"));
                if (!dir.Exists)
                    dir.Create();

               string logoPath=System.IO.Path.Combine(dir.FullName, partsFileName[partsFileName.Length - 1]);

                using (var fileStream = new FileStream(logoPath, FileMode.Create))
                {
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(image));
                    encoder.Save(fileStream);
                }
                logoUrl = logoPath;
                logoImg.Source = new BitmapImage(new Uri(logoPath));

            }
        }

        private async void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(txtShopName.Text.Trim()=="")
                {
                    await this.ShowMessageAsync("Settings", "Shop Name Cannot be empty !");
                    return;
                }
                if (txtContact.Text.Trim() == "")
                {
                    await this.ShowMessageAsync("Settings", "Contact No Cannot be empty !");
                    return;
                }
                if (txtAddress.Text.Trim() == "")
                {
                    await this.ShowMessageAsync("Settings", "Shop Address Cannot be empty !");
                    return;
                }
                LogoUpload();
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("Settings", ex.Message);
            }
            
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
        private void mnProduct_Click(object sender, RoutedEventArgs e)
        {
            ProductWindow cat = new ProductWindow();
            cat.Show();
            this.Close();
        }
    }
}
