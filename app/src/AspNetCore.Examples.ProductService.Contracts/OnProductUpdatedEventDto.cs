using System;

namespace AspNetCore.Examples.ProductService
{
    public sealed class OnProductUpdatedEventDto
    {
        public Guid Id { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
    }
}