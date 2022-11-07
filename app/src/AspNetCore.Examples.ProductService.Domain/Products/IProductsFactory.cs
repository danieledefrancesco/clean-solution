using System;
using AspNetCore.Examples.ProductService.Products;

namespace AspNetCore.Examples.ProductService.Factories
{
    public interface IProductsFactory
    {
        Product CreateProduct(ProductId productId, Action<Product> initialize = null);
    }
}