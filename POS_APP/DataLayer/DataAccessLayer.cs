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
                return -1;
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
                return -1;
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
                return -1;
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
                return null;
            }
        }
        public async Task<Category> GetOneCategory(int categoryId)
        {
            try
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
                    return null;
                }

            }
            catch (Exception)
            {

                throw;
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
                return null;
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
            catch (Exception)
            {
                return -1;
            }
        }

        public async Task<IEnumerable<Products>> GetProducts()
        {
            try
            {
                using (var context = new POS_DB())
                {
                    var Listcat = context.Products.Include(x=>x.Category).Where(x => x.IsDeleted == false).ToList();

                    return await Task.FromResult(Listcat);
                }
            }
            catch (Exception ex)
            {
                return null;
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
                return -1;
            }
        }

        public async Task<Products> GetOneProduct(int productId)
        {
            try
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
                    return null;
                }

            }
            catch (Exception)
            {

                throw;
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
                return -1;
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
                return null;
            }
        }
    }
}
