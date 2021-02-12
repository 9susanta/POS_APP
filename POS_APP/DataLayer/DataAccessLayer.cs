using POS_APP.Data;
using POS_APP.Data.Tables;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_APP.DataLayer
{
    public class DataAccessLayer
    {
        public int AddCategory(Category category)
        {
            try
            {
                using (var context = new POS_DB())
                {
                    var catExist = context.Category.Where(x => x.Name == category.Name).FirstOrDefault();

                    if (catExist == null)
                    {

                        context.Category.Add(category);

                        return context.SaveChanges();
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int UpdateCategory(Category category)
        {
            try
            {
                using (var context = new POS_DB())
                {
                    var cat = context.Category.Where(x => x.Id == category.Id).FirstOrDefault();
                    cat.Name = category.Name;
                    cat.IsDeleted = category.IsDeleted;
                    context.Category.Attach(cat);
                    context.Entry(cat).State = EntityState.Modified;
                    return context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int DeleteCategory(Category category)
        {
            try
            {
                using (var context = new POS_DB())
                {
                    var cat = context.Category.Where(x => x.Id == category.Id).FirstOrDefault();
                    cat.IsDeleted = category.IsDeleted;
                    context.Category.Attach(cat);
                    context.Entry(cat).State = EntityState.Modified;
                    return context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<Category>> GetCategories()
        {
            try
            {
                using (var context = new POS_DB())
                {
                    var Listcat = context.Category.Where(x => x.IsDeleted == false).ToList();

                    return await Task.FromResult(Listcat);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Category> GetOneCategory(int categoryId)
        {
             try
             {
                    using (var context = new POS_DB())
                    {
                        var lastcat = context.Category.Where(x => x.Id == categoryId).FirstOrDefault();

                        return await Task.FromResult(lastcat);
                    }
             }
             catch (Exception ex)
             {
                throw ex;
             }
        }
        public async Task<Products> GetLastProductData()
        {

            try
            {
                using (var context = new POS_DB())
                {
                    var lastcat = context.Products.Where(x => x.IsDeleted == false).OrderByDescending(x=>x.ProductsId).FirstOrDefault();

                    return await Task.FromResult(lastcat);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddProduct(Products product)
        {
            try
            {
                using (var context = new POS_DB())
                {
                    var catExist = context.Products.Where(x => x.ProdName == product.ProdName).FirstOrDefault();

                    if (catExist == null)
                    {

                        context.Products.Add(product);

                        return context.SaveChanges();
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Products>> GetProducts()
        {
            try
            {
                using (var context = new POS_DB())
                {
                    var Listcat = context.Products.Include(x=>x.Category).Where(x => x.IsDeleted == false).OrderByDescending(x=>x.ProductsId).ToListAsync();

                    return await Listcat;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int RemoveProduct(int productId)
        {
            try
            {
                using (var context = new POS_DB())
                {
                    var product = context.Products.Where(x => x.ProductsId == productId).FirstOrDefault();
                    context.Products.Remove(product);
                    return context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Products> GetOneProduct(int productId)
        {
            try
            {
               using (var context = new POS_DB())
               {
                    var lastcat = context.Products.Where(x => x.ProductsId == productId).FirstOrDefault();

                    return await Task.FromResult(lastcat);
               }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int UpdateProducts(Products products)
        {
            try
            {
                using (var context = new POS_DB())
                {
                    var pro = context.Products.Where(x => x.ProdName == products.ProdName&&x.ProductsId!=products.ProductsId).FirstOrDefault();
                    if(pro!=null)
                    {
                        return 0;
                    }
                    context.Products.Attach(products);
                    context.Entry(products).State = EntityState.Modified;
                    return context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Products>> GetProductsByCategory(int categoryId)
        {
            try
            {
                using (var context = new POS_DB())
                {
                    if (categoryId == 0)
                    {
                        var Listcat = context.Products.Include(x => x.Category).Where(x => x.IsDeleted == false).ToList();

                        return await Task.FromResult(Listcat);
                    }
                    else
                    {
                        var Listcat = context.Products.Include(x => x.Category).Where(x => x.IsDeleted == false&&x.Category.Id==categoryId).ToList();

                        return await Task.FromResult(Listcat);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateCompanies(Company _company)
        {
            try
            {
                using (var context = new POS_DB())
                {
                    var company = context.Companies.FirstOrDefault();

                    if (company == null)
                    {
                        company = _company;
                        context.Companies.Add(company);
                    }
                    else
                    {
                        company.ShopName = _company.ShopName;
                        company.ShopAddress = _company.ShopAddress;
                        company.ImageUrl = _company.ImageUrl;
                        company.ContactNo = _company.ContactNo;
                        context.Companies.Attach(company);
                        context.Entry(company).State = EntityState.Modified;
                    }

                    return context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdatePrinter(Company _company)
        {
            try
            {
                using (var context = new POS_DB())
                {
                    var company = context.Companies.FirstOrDefault();

                    if (company != null)
                    {
                        company.PrinterName = _company.PrinterName;
                        context.Companies.Attach(company);
                        context.Entry(company).State = EntityState.Modified;
                    }

                    return context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Company GetCompany()
        {
            try
            {
                using (var context = new POS_DB())
                {
                    return context.Companies.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int SaveCustomer(Customers customers)
        {
            try
            {
                using (var context = new POS_DB())
                {
                    Customers cust = new Customers();
                    cust.CName = customers.CName;
                    cust.CPhone = customers.CPhone;
                    cust.PurchaseDate = DateTime.Now;
                    context.Customers.Add(cust);

                    return context.SaveChanges();

                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public int Invoice()
        {
            try
            {
                using (var context = new POS_DB())
                {

                    var count = context.Customers.OrderByDescending(x => x.Id).ToList().Count;
                    return count;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
