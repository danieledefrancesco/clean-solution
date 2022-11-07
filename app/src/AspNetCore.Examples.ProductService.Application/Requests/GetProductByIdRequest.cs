using AspNetCore.Examples.ProductService.Responses;
using AspNetCore.Examples.ProductService.ValueObjects;

namespace AspNetCore.Examples.ProductService.Requests
{
    public sealed class GetProductByIdRequest : IAppRequest<GetProductByIdResponse>
    {
        public GetProductByIdRequest(ProductId productId)
        {
            ProductId = productId;
        }

        public ProductId ProductId { get; }
    }
}