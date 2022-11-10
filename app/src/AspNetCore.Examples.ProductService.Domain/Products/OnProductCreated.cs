using System;
using AspNetCore.Examples.ProductService.Events;

namespace AspNetCore.Examples.ProductService.Products
{
    public sealed class OnProductCreated : DomainEventBase
    {
        public OnProductCreated(Guid id, ProductId productId, ProductName productName, ProductPrice productPrice): base(id)
        {
            ProductId = productId;
            ProductName = productName;
            ProductPrice = productPrice;
        }

        public ProductId ProductId { get; }
        public ProductName ProductName { get; }
        public ProductPrice ProductPrice { get; }
    }
}