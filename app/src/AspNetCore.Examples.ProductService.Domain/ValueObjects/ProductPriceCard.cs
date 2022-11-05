namespace AspNetCore.Examples.ProductService.ValueObjects
{
    public readonly record struct ProductPriceCard(string PriceCardId, ProductPrice NewPrice)
    {
    }
}