using System;
using AspNetCore.Examples.ProductService.Entities;
using AspNetCore.Examples.ProductService.ValueObjects;

namespace AspNetCore.Examples.ProductService.Factories
{
    public interface IProductsFactory
    {
        Product CreateProduct(ProductId productId, Action<Product> initialize = null);
    }
}