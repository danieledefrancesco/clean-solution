using System;

namespace AspNetCore.Examples.ProductService.Products
{
    public sealed class ProductsFactory: IProductsFactory
    {
        public Product CreateProduct(ProductId productId, ProductName productName, ProductPrice productPrice)
        {
            var product = new Product(productId, productName, productPrice);
            product.AddDomainEvent(new OnProductCreated(Guid.NewGuid(), product.Id, product.Name, product.Price));
            return product;
        }
    }
}