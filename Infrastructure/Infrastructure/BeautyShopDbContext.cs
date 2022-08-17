using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.EntityConfigurations;

namespace Infrastructure
{
    public class BeautyShopDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Customer> Customers  { get; set; }
        public DbSet<Product> Products  { get; set; }
        public DbSet<Purchase> Purchases  { get; set; }
        public DbSet<ProductInCart> ProductInCarts { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        public BeautyShopDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Category>(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration<Comment>(new CommentConfiguration());
            modelBuilder.ApplyConfiguration<Customer>(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration<Product>(new ProductsConfiguration());
            modelBuilder.ApplyConfiguration<Purchase>(new PurchaseConfiguration());
            modelBuilder.ApplyConfiguration<ProductInCart>(new ProductInCartConfiguration());
            modelBuilder.ApplyConfiguration<ShoppingCart>(new ShoppingCartConfiguration());
        }

    }
}
