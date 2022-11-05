using AspNetCore.Examples.ProductService.Entities;
using AspNetCore.Examples.ProductService.ValueObjects;

namespace AspNetCore.Examples.ProductService.Repositories
{
    public interface IProductRepository : IRepository<Product, ProductId>
    {
        
    }
}