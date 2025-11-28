using Microsoft.EntityFrameworkCore;
using Yunu.Api.Domain;

namespace Yunu.Api.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Cabinet> Cabinet { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Status> CurrentStatus { get; set; }
        public DbSet<Delivery> Delivery { get; set; }
        public DbSet<Order> Order { get; set; }

        public DbSet<Product> Product { get; set; }        
        public DbSet<Product_Fbo_Stocks> Fbo_Stocks { get; set; }
        public DbSet<Product_Fbo_Stocks_By_Delivery_Type> By_Delivery_Type { get; set; }
        public DbSet<Product_Marketplaces> Marketplaces { get; set; }

        public DbSet<TransportCompany> TransportCompany { get; set; }
        public DbSet<Warehouse> Warehouse { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cabinet>().ToTable("Cabinet").HasKey(e => e.id);
            modelBuilder.Entity<Cabinet>().Property(e => e.id).ValueGeneratedNever();

            modelBuilder.Entity<Category>().ToTable("Category").HasKey(e => e.id);
            modelBuilder.Entity<Category>().Property(e => e.id).ValueGeneratedNever();

            modelBuilder.Entity<Status>().ToTable("Status").HasKey(e => e.id);
            modelBuilder.Entity<Status>().Property(e => e.id).ValueGeneratedNever();

            modelBuilder.Entity<Delivery>().ToTable("Delivery").HasKey(e => e.id);
            modelBuilder.Entity<Delivery>().Property(e => e.id).ValueGeneratedNever();
            modelBuilder.Entity<Delivery>().OwnsOne(e => e.address);

            modelBuilder.Entity<Order>().ToTable("Order").HasKey(e => new { e.id, e.uid });
            modelBuilder.Entity<Order>().Property(e => e.id).ValueGeneratedNever();
            modelBuilder.Entity<Order>().OwnsOne(e => e.consumer);

            modelBuilder.Entity<Product>().ToTable("Product").HasKey(e => e.id);
            modelBuilder.Entity<Product>().Property(e => e.id).ValueGeneratedNever();
            modelBuilder.Entity<Product>().HasOne(e => e.fbo_stocks).WithOne().HasForeignKey<Product_Fbo_Stocks>(e => e.ProductId).HasPrincipalKey<Product>(e => e.id);
            modelBuilder.Entity<Product>().HasOne(e => e.marketplaces).WithOne().HasForeignKey<Product_Marketplaces>(e => e.ProductId).HasPrincipalKey<Product>(e => e.id);

            modelBuilder.Entity<Product_Fbo_Stocks>().ToTable("Product_Fbo_Stocks").HasKey(e => e.ProductId);
            modelBuilder.Entity<Product_Fbo_Stocks>().Property(e => e.ProductId).ValueGeneratedNever();
            modelBuilder.Entity<Product_Fbo_Stocks>().HasMany(e => e.by_delivery_type).WithOne().HasForeignKey(e => e.ProductId).HasPrincipalKey(e => e.ProductId);

            modelBuilder.Entity<Product_Fbo_Stocks_By_Delivery_Type>().ToTable("Product_Fbo_Stocks_By_Delivery_Type").HasKey(e => new { e.ProductId, e.delivery_type_name });
            modelBuilder.Entity<Product_Fbo_Stocks_By_Delivery_Type>().Property(e => e.ProductId).ValueGeneratedNever();

            modelBuilder.Entity<Product_Marketplaces>().ToTable("Product_Marketplaces").HasKey(e => e.ProductId);
            modelBuilder.Entity<Product_Marketplaces>().Property(e => e.ProductId).ValueGeneratedNever();

            modelBuilder.Entity<TransportCompany>().ToTable("TransportCompany").HasKey(e => e.id);
            modelBuilder.Entity<TransportCompany>().Property(e => e.id).ValueGeneratedNever();

            modelBuilder.Entity<Warehouse>().ToTable("Warehouse").HasKey(e => e.id);
            modelBuilder.Entity<Warehouse>().Property(e => e.id).ValueGeneratedNever();

            base.OnModelCreating(modelBuilder);
        }
    }
}
