using AspNetCore.Examples.ProductService.Entities;

namespace AspNetCore.Examples.ProductService.Products
{
    public sealed class Product : AggregateRootBase<ProductId>
    {
        public Product(ProductId id) : base(id)
        {
        }
        
        public ProductName Name { get; set; }
        public ProductPrice Price { get; set; }

        public ProductWithPriceCard ApplyPriceCard(ProductPriceCard? priceCard)
        {
            return new ProductWithPriceCard(this, priceCard);
        }

    }
}