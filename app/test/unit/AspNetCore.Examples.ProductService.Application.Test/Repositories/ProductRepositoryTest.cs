using AspNetCore.Examples.ProductService.Entities;
using AspNetCore.Examples.ProductService.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Examples.ProductService.Repositories
{
    public sealed class ProductRepositoryTest : RepositoryTestBase<Product,ProductId>
    {
        protected override RepositoryBase<Product, ProductId> CreteRepository(DbContext dbContext)
        {
            return new ProductRepository(dbContext);
        }

        protected override DbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(nameof(AppDbContext))
                .Options;
            return new AppDbContext(options);

        }

        protected override Product CreateTestEntity()
        {
            return new Product(ProductId.From("productId"))
            {
                Name =  ProductName.From("productName")
            };
        }

        protected override ProductId CreateId()
        {
            return ProductId.From("id");
        }
    }
}