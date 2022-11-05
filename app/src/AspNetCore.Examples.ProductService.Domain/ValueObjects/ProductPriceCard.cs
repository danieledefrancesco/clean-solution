using System;
using AspNetCore.Examples.ProductService.Services;

namespace AspNetCore.Examples.ProductService.ValueObjects
{
    public readonly record struct ProductPriceCard(string PriceCardId, ProductPrice NewPrice)
    {
    }
}