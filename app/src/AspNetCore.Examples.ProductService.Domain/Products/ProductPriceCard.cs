namespace AspNetCore.Examples.ProductService.Products
{
    public readonly record struct ProductPriceCard(string PriceCardId, ProductPrice NewPrice)
    {
    }
}