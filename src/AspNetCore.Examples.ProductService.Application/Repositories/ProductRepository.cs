using AspNetCore.Examples.ProductService.Entities;
using AspNetCore.Examples.ProductService.Persistence;

namespace AspNetCore.Examples.ProductService.Repositories
{
    public class ProductRepository : RepositoryBase<Product, string>, IProductRepository
    {
        public ProductRepository(IPersistenceImplementation<Product, string> persistenceImplementation) : base(persistenceImplementation)
        {
        }
    }
}