using AspNetCore.Examples.ProductService.Entities;
using AspNetCore.Examples.ProductService.Persistence;
using AspNetCore.Examples.ProductService.ValueObjects;

namespace AspNetCore.Examples.ProductService.Repositories
{
    public class ProductRepositoryTest : RepositoryTestBase<Product,string>
    {
        protected override RepositoryBase<Product, string> CreteRepository(IPersistenceImplementation<Product, string> persistenceImplementation)
        {
            return new ProductRepository(persistenceImplementation);
        }

        protected override Product CreateTestEntity()
        {
            return new Product()
            {
                Id = "productId",
                Name =  ProductName.From("productName")
            };
        }
    }
}