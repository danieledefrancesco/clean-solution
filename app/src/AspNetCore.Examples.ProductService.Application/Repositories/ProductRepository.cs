using AspNetCore.Examples.ProductService.Entities;
using AspNetCore.Examples.ProductService.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Examples.ProductService.Repositories
{
    public class ProductRepository : RepositoryBase<Product, string>, IProductRepository
    {
        public ProductRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}