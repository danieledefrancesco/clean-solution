using AspNetCore.Examples.ProductService.Repositories;

namespace AspNetCore.Examples.ProductService.Products
{
    public interface IProductRepository : IRepository<Product, ProductId>
    {
        
    }
}