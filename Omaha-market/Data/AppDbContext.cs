using Omaha_market.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Configuration;

namespace Omaha_market.Data
{
    public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
       
        public DbSet<AccountModel> Accounts  { get; set; } = null!;
        //public DbSet<ProductModel> Products { get; set; } = null!;
       
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
           Database.EnsureCreated();
        }
    }
}
