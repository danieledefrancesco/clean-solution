using AspNetCore.Examples.ProductService.Responses;

namespace AspNetCore.Examples.ProductService.Requests
{
    public class GetProductByIdRequest : IAppRequest<GetProductByIdResponse>
    {
        public GetProductByIdRequest(string productId)
        {
            ProductId = productId;
        }

        public string ProductId { get; }
        
    }
}