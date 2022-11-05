using AspNetCore.Examples.ProductService.Responses;
using AspNetCore.Examples.ProductService.ValueObjects;

namespace AspNetCore.Examples.ProductService.Requests
{
    public sealed class GetProductWithPriceCardByIdRequest : IAppRequest<GetProductWithPriceCardByIdResponse>
    {
        public GetProductWithPriceCardByIdRequest(ProductId productId)
        {
            ProductId = productId;
        }

        public ProductId ProductId { get; }
        
    }
}