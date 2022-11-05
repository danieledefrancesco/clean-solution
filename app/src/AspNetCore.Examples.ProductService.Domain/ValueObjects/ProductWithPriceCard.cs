using AspNetCore.Examples.ProductService.Entities;

namespace AspNetCore.Examples.ProductService.ValueObjects
{
    public readonly record struct ProductWithPriceCard(Product Product, ProductPriceCard? PriceCard)
    {
        public ProductPrice FinalPrice => PriceCard?.NewPrice ?? Product.Price;
    }

}