using Microsoft.EntityFrameworkCore;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.DataAccess
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<FilePath> FilePaths { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Bill> Bills { get; set; }

        public DbSet<BillDetails> BillDetails { get; set; }
    }
}
