using AspNetCore.Examples.ProductService.Entities;
using AspNetCore.Examples.ProductService.Products;
using Microsoft.EntityFrameworkCore;
using ZapMicro.TransactionalOutbox.DbContexts;
using ZapMicro.TransactionalOutbox.Entities;

namespace AspNetCore.Examples.ProductService
{
    public sealed class AppDbContext: DbContext, ITransactionalOutboxDbContext
    {
        public AppDbContext() : this(Utils.GetSqlServerDbContextOptions())
        {
            
        }
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Product>()
                .Property(product => product.Id)
                .HasConversion(productId => productId.Value,
                    productIdString => ProductId.From(productIdString));
            
            modelBuilder.Entity<Product>()
                .Property(product => product.Name)
                .HasConversion(productName => productName.Value,
                    productNameString => ProductName.From(productNameString));
            
            modelBuilder.Entity<Product>()
                .Property(product => product.Price)
                .HasConversion(productPrice => productPrice.Value,
                    productPriceDecimal => ProductPrice.From(productPriceDecimal));
            
            modelBuilder.Entity<Product>()
                .HasKey(product => product.Id);
        }
    }
}