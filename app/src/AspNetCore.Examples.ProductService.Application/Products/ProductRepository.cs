using AspNetCore.Examples.ProductService.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Examples.ProductService.Products
{
    public sealed class ProductRepository : RepositoryBase<Product, ProductId>, IProductRepository
    {
        public ProductRepository(DbContext dbContext) : base(dbContext)
        {
        }

    }
}