using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVC_Assignment.Models
{
    public class ShoppingCartContext : DbContext
    {
        public ShoppingCartContext() : base("MyConnectionString")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ShoppingCartContext, MVC_Assignment.Migrations.Configuration>());
        }
        public DbSet<User> Users { get; set; }  
        public DbSet<Product> Products { get; set; }
    }
}