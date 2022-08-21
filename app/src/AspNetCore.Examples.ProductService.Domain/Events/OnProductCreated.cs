using AspNetCore.Examples.ProductService.Entities;

namespace AspNetCore.Examples.ProductService.Events
{
    public class OnProductCreated : EventBase
    {
        public Product CreatedProduct { get; set; }
    }
}