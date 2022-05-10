using AspNetCore.Examples.ProductService.Common;
using AspNetCore.Examples.ProductService.ValueObjects;

namespace AspNetCore.Examples.ProductService.Entities
{
    public class Product : EntityBase<string>
    {
        public ProductName Name { get; set; }
        public ProductPrice Price { get; set; }
    }
}