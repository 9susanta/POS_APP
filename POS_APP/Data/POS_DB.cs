using POS_APP.Data.Tables;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace POS_APP.Data
{
    public class POS_DB:DbContext
    {
        public POS_DB() : base("POSDBContext")
        {
        }

        public DbSet<Category> Category { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<SellRecords> SellRecords { get; set; }
        public DbSet<Company> Companies { get; set; }
    }
}
