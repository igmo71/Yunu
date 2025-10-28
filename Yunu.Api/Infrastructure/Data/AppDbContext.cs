using Microsoft.EntityFrameworkCore;
using Yunu.Api.Domain;

namespace Yunu.Api.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Product { get; set; }
        public DbSet<Product_Fbo_Stock> Fbo_Stock { get; set; }
        public DbSet<Product_Fbo_Stock_By_Delivery_Type> Fbo_Stock_By_Delivery_Type { get; set; }
        public DbSet<Product_Fbo_Stocks> Fbo_Stocks { get; set; }
        public DbSet<Product_Fbo_Stocks_By_Delivery_Type> Fbo_Stocks_By_Delivery_Type { get; set; }
        public DbSet<Product_Marketplaces> Product_Marketplaces { get; set; }

        public DbSet<Category> Category { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("Product").HasKey(e => e.id);
            modelBuilder.Entity<Product>().Property(e => e.id).ValueGeneratedNever();

            modelBuilder.Entity<Product_Fbo_Stock>().ToTable("Product_Fbo_Stock").HasKey(e => e.ProductId);
            modelBuilder.Entity<Product_Fbo_Stock>().Property(e => e.ProductId).ValueGeneratedNever();
            modelBuilder.Entity<Product_Fbo_Stock>().HasMany(e => e.by_delivery_type).WithOne().HasForeignKey(e => e.ProductId).HasPrincipalKey(e => e.ProductId);

            modelBuilder.Entity<Product_Fbo_Stock_By_Delivery_Type>().ToTable("Product_Fbo_Stock_By_Delivery_Type").HasKey(e => new { e.ProductId, e.delivery_type_name });
            modelBuilder.Entity<Product_Fbo_Stock_By_Delivery_Type>().Property(e => e.ProductId).ValueGeneratedNever();


            modelBuilder.Entity<Product_Fbo_Stocks>().ToTable("Product_Fbo_Stocks").HasKey(e => e.ProductId);
            modelBuilder.Entity<Product_Fbo_Stocks>().Property(e => e.ProductId).ValueGeneratedNever();
            modelBuilder.Entity<Product_Fbo_Stocks>().HasMany(e => e.by_delivery_type).WithOne().HasForeignKey(e => e.ProductId).HasPrincipalKey(e => e.ProductId);

            modelBuilder.Entity<Product_Fbo_Stocks_By_Delivery_Type>().ToTable("Product_Fbo_Stocks_By_Delivery_Type").HasKey(e => new { e.ProductId, e.delivery_type_name });
            modelBuilder.Entity<Product_Fbo_Stocks_By_Delivery_Type>().Property(e => e.ProductId).ValueGeneratedNever();

            modelBuilder.Entity<Product_Marketplaces>().ToTable("Product_Marketplaces").HasKey(e => e.ProductId);
            modelBuilder.Entity<Product_Marketplaces>().Property(e => e.ProductId).ValueGeneratedNever();

            modelBuilder.Entity<Category>().ToTable("Category").HasKey(e => e.id);
            modelBuilder.Entity<Category>().Property(e => e.id).ValueGeneratedNever();

            base.OnModelCreating(modelBuilder);
        }
    }
}
