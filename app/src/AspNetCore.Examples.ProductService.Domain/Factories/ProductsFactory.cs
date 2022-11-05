using System;
using AspNetCore.Examples.ProductService.Entities;
using AspNetCore.Examples.ProductService.Events;
using AspNetCore.Examples.ProductService.ValueObjects;

namespace AspNetCore.Examples.ProductService.Factories
{
    public sealed class ProductsFactory: IProductsFactory
    {
        public Product CreateProduct(ProductId productId, Action<Product> initialize = null)
        {
            var product = new Product(productId);
            initialize?.Invoke(product);
            product.AddDomainEvent(new OnProductCreated(Guid.NewGuid(), product.Id, product.Name, product.Price));
            return product;
        }
    }
}