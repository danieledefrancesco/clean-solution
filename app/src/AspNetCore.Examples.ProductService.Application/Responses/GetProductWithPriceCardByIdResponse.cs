using AspNetCore.Examples.ProductService.ValueObjects;

namespace AspNetCore.Examples.ProductService.Responses
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