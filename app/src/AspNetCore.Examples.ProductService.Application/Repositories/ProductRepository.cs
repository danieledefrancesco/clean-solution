using AspNetCore.Examples.ProductService.Entities;
using AspNetCore.Examples.ProductService.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Examples.ProductService.Repositories
{
    public sealed class ProductRepository : RepositoryBase<Product, ProductId>, IProductRepository
    {
        public ProductRepository(DbContext dbContext) : base(dbContext)
        {
        }

    }
}