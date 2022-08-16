using AspNetCore.Examples.ProductService.Entities;
using AspNetCore.Examples.ProductService.Persistence;
using AspNetCore.Examples.ProductService.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Examples.ProductService.Repositories
{
    public class ProductRepositoryTest : RepositoryTestBase<Product,string>
    {
        protected override RepositoryBase<Product, string> CreteRepository(DbContext dbContext)
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
            return new Product
            {
                Id = "productId",
                Name =  ProductName.From("productName")
            };
        }

        protected override string CreateId()
        {
            return "id";
        }
    }
}