using AspNetCore.Examples.ProductService.Entities;

namespace AspNetCore.Examples.ProductService.Repositories
{
    public interface IProductRepository : IRepository<Product,string>
    {
        
    }
}