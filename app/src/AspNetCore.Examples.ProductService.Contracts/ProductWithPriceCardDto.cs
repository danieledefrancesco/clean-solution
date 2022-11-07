namespace AspNetCore.Examples.ProductService
{
    public sealed class ProductWithPriceCardDto: ProductDto
    {
        public decimal FinalPrice { get; set; }
        public PriceCardDto PriceCard { get; set; }
    }
}