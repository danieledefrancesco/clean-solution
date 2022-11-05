using System;
using ValueOf;

namespace AspNetCore.Examples.ProductService.ValueObjects
{
    public sealed class ProductName : ValueOf<string, ProductName>
    {
        protected override void Validate()
        {
            if (string.IsNullOrWhiteSpace(Value))
            {
                throw new ArgumentException($"{Value} is an invalid ${nameof(ProductName)}");
            }
        }
    }
}