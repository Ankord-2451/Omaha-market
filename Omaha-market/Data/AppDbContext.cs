using Omaha_market.Models;
using Microsoft.EntityFrameworkCore;

namespace Omaha_market.Data
{
    public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
       
        public DbSet<AccountModel> Accounts  { get; set; } = null!;
        public DbSet<ProductModel> Products { get; set; } = null!;
        public DbSet<CartModel> ShoppingCart { get; set; } = null!;
        public DbSet<favoriteModel> favorite { get; set; } = null!;
        public DbSet<CategoryModel> Category { get; set; } = null!;
        public DbSet<EmailModel> Email { get; set; } = null!;
        public DbSet<TextModel> Text { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        
        }
    }
}
