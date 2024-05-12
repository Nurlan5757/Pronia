using Microsoft.EntityFrameworkCore;
using Pronia.Models;

namespace Pronia.DataAccesLayer
{
    public class ProniaContext : DbContext
    {
        public ProniaContext(DbContextOptions options) : base(options) 
        {            
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<ProductImage> Images { get; set; }
        public DbSet<Product> Product { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Server=DESKTOP-8ABCEA9\\SQLEXPRESS;Database=Pronia;Trusted_Connection=true;TrustServerCertificate=True");
            base.OnConfiguring(options);
        }
    }
}
