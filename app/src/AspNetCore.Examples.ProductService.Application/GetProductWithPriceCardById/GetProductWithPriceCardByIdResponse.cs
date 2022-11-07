using AspNetCore.Examples.ProductService.Products;

namespace AspNetCore.Examples.ProductService.GetProductWithPriceCardById
{
    public sealed class GetProductWithPriceCardByIdResponse
    {
        public GetProductWithPriceCardByIdResponse(ProductWithPriceCard productWithPriceCard)
        {
            ProductWithPriceCard = productWithPriceCard;
        }

        public ProductWithPriceCard ProductWithPriceCard { get; }
        
    }
}