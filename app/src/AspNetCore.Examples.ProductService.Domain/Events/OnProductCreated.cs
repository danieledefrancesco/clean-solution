using System;
using AspNetCore.Examples.ProductService.ValueObjects;

namespace AspNetCore.Examples.ProductService.Events
{
    public sealed class OnProductCreated : DomainEventBase
    {
        public OnProductCreated(Guid id, ProductId productId, ProductName productName, ProductPrice productPrice)
        {
            Id = id;
            ProductId = productId;
            ProductName = productName;
            ProductPrice = productPrice;
        }

        public ProductId ProductId { get; }
        public ProductName ProductName { get; }
        public ProductPrice ProductPrice { get; }
    }
}