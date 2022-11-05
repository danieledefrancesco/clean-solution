using System;
using ValueOf;

namespace AspNetCore.Examples.ProductService.ValueObjects
{
    public sealed class ProductPrice : ValueOf<decimal, ProductPrice>
    {
        protected override void Validate()
        {
            if (Value < 0 )
            {
                throw new ArgumentException($"{Value} is an invalid ${nameof(ProductPrice)}. It should be greater or equal to 0.");
            }
        }
    }
}