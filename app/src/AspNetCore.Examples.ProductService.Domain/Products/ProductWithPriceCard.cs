namespace AspNetCore.Examples.ProductService.Products
{
    public readonly record struct ProductWithPriceCard(Product Product, ProductPriceCard? PriceCard)
    {
        public ProductPrice FinalPrice => PriceCard?.NewPrice ?? Product.Price;
    }

}