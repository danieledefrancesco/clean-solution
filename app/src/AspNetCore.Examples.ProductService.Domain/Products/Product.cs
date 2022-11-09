using System;
using AspNetCore.Examples.ProductService.Entities;

namespace AspNetCore.Examples.ProductService.Products
{
    public sealed class Product : AggregateRootBase<ProductId>
    {
        public Product(ProductId id, ProductName name, ProductPrice price) : base(id)
        {
            Name = name;
            Price = price;
        }
        
        public ProductName Name { get; private set; }
        public ProductPrice Price { get; private set; }

        public ProductWithPriceCard ApplyPriceCard(ProductPriceCard? priceCard)
        {
            return new ProductWithPriceCard(this, priceCard);
        }

        public void Update(ProductName newName, ProductPrice newPrice)
        {
            Name = newName;
            Price = newPrice;
            AddDomainEvent(new OnProductUpdated(Guid.NewGuid(), Id, Name, Price));
        }
    }
}