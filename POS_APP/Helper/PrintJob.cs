using POS_APP.Data.Tables;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_APP.Helper
{
    public class PrintJob
    {
        private PrintDocument PrintDocument;
        private Graphics graphics;
        private int InitialHeight = 360;
        private Order order { set; get; }
        private Company shop { set; get; }
        public PrintJob(Order order, Company shop)
        {
            this.order = order;
            this.shop = shop;
            AdjustHeight();
        }
        private void AdjustHeight()
        {
            var capacity = 5 * order.lstInvoice.Count;
            InitialHeight += capacity;
        }
        public void Print(string printername)
        {
            PrintDocument = new PrintDocument();
            PrintDocument.PrinterSettings.PrinterName = printername;

            PrintDocument.PrintPage += new PrintPageEventHandler(FormatPage);
            PrintDocument.Print();
        }
        void DrawAtStart(string text, int Offset)
        {
            int startX = 10;
            int startY = 5;
            Font minifont = new Font("Arial", 7);

            graphics.DrawString(text, minifont,
                     new SolidBrush(Color.Black), startX + 5, startY + Offset);
        }
        void InsertItem(string key, string value, int Offset)
        {
            Font minifont = new Font("Arial", 7);
            int startX = 10;
            int startY = 5;

            graphics.DrawString(key, minifont,
                         new SolidBrush(Color.Black), startX + 5, startY + Offset);

            graphics.DrawString(value, minifont,
                     new SolidBrush(Color.Black), startX + 120, startY + Offset);
        }
        void InsertHeaderStyleItem(string key, string value, int Offset)
        {
            int startX = 10;
            int startY = 5;
            Font itemfont = new Font("Arial", 7, FontStyle.Bold);

            graphics.DrawString(key, itemfont,
                         new SolidBrush(Color.Black), startX + 5, startY + Offset);

            graphics.DrawString(value, itemfont,
                     new SolidBrush(Color.Black), startX + 130, startY + Offset);
        }
        void DrawLine(string text, Font font, int Offset, int xOffset)
        {
            int startX = 10;
            int startY = 5;
            graphics.DrawString(text, font,
                     new SolidBrush(Color.Black), startX + xOffset, startY + Offset);
        }
        void DrawSimpleString(string text, Font font, int Offset, int xOffset)
        {
            int startX = 10;
            int startY = 5;
            graphics.DrawString(text, font,
                     new SolidBrush(Color.Black), startX + xOffset, startY + Offset);
        }
        private void FormatPage(object sender, PrintPageEventArgs e)
        {
            graphics = e.Graphics;
            Font minifont = new Font("Arial", 6);
            Font itemfont = new Font("Arial", 7);
            Font smallfont = new Font("Arial", 8);
            Font mediumfont = new Font("Arial", 9);
            Font largefont = new Font("Arial", 10);
            int Offset = 10;
            int smallinc = 10, mediuminc = 12, largeinc = 15;

            //if (shop.ImageUrl != null)
            //{
            //    try
            //    {
            //        Image image = Image.FromFile(shop.ImageUrl);
            //        e.Graphics.DrawImage(image, 10 + 50, 5 + Offset, 100, 30);
            //    }
            //    catch (Exception ex)
            //    {
            //    }
                
            //}

            //Offset = Offset + largeinc + 20;

            graphics.DrawString("Welcome to "+shop.ShopName,largefont,new SolidBrush(Color.Black), 20,0);

            Offset = Offset + largeinc-20 ;

            String underLine = "-------------------------------------";
            DrawLine(underLine, largefont, Offset, 0);

            Offset = Offset + mediuminc;
            DrawAtStart("Invoice Number #: " + order.Invoice, Offset);

            Offset = Offset + mediuminc;
            DrawAtStart("Customer Name : " + order.CustomerName, Offset);

            Offset = Offset + 5;
            if (!String.Equals(shop.ShopAddress, ""))
            {
                var shopAdd = shop.ShopAddress.Split('\n');
                for(int saId=0;saId< shopAdd.Length;saId++)
                {
                    Offset = Offset + mediuminc;
                    if (saId == 0)
                    {
                        DrawAtStart("Address: " + shopAdd[saId], Offset);
                    }
                    else
                    {
                        DrawAtStart(shopAdd[saId], Offset);
                    }
                }
            }

            if (!String.Equals(shop.ContactNo, ""))
            {
                Offset = Offset + mediuminc;
                DrawAtStart("Phone # : " + shop.ContactNo, Offset);
            }

            Offset = Offset + mediuminc;
            DrawAtStart("Date: " + order.OrderDate, Offset);

            Offset = Offset + smallinc-3;
            underLine = "-------------------------------------";
            DrawLine(underLine, largefont, Offset, 0);

            Offset = Offset + largeinc-5;

            InsertHeaderStyleItem("Name. ", "Price. ", Offset+5);

            Offset = Offset + largeinc+5;
            foreach (var itran in order.lstInvoice)
            {
                InsertItem(itran.ProdName.Trim().
                    Substring(0, itran.ProdName.Trim().Length > 5 
                    ? 5 : itran.ProdName.Trim().Length)+"_"+ itran.CategoryName.Trim().
                    Substring(0, itran.CategoryName.Trim().Length > 5
                    ? 5 : itran.CategoryName.Trim().Length) + " x " + itran.Qty, itran.Total.ToString("C"), Offset);
                Offset = Offset + mediuminc;
            }

            underLine = "-------------------------------------";
            DrawLine(underLine, largefont, Offset-5, 0);

            Offset = Offset + largeinc-5;
            InsertItem(" Net. Total: ", order.SubTotal.ToString(), Offset);
            #region Discount
            //if (!order.Cash.Discount.IsZero())
            //{
            //    Offset = Offset + smallinc;
            //    InsertItem(" Discount: ", order.Cash.Discount.CValue, Offset);
            //}

            //Offset = Offset + smallinc;
            //InsertHeaderStyleItem(" Amount Payable: ", order.GrossTotal.CValue, Offset);
            #endregion
            //Offset = Offset + largeinc;
            //String address = shop.ShopAddress;
            //DrawSimpleString("Address: " + address, minifont, Offset, 15);

            Offset = Offset + largeinc+2;
            String number = "We Accept All Types of Card !!!";
            DrawSimpleString(number, minifont, Offset, 15);

            Offset = Offset + largeinc-6;
            underLine = "-------------------------------------";
            DrawLine(underLine, largefont, Offset, 0);

            Offset = Offset + mediuminc+3;
            String greetings = "Thanks for visiting us.";
            DrawSimpleString(greetings, smallfont, Offset, 10);

            Offset = Offset + mediuminc-3;
            underLine = "-------------------------------------";
            DrawLine(underLine, largefont, Offset, 0);

            Offset += (2 * mediuminc);
            string tip = "TIP: -----------------------------";
            InsertItem(tip, "", Offset);

            //Offset = Offset + largeinc;
            //string DrawnBy = "Meganos Softwares: 0312-0459491 - OR - 0321-6228321";
            //DrawSimpleString(DrawnBy, minifont, Offset, 15);
        }
    }
}
