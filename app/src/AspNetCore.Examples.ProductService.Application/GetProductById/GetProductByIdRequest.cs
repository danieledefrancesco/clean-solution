using AspNetCore.Examples.ProductService.Products;
using AspNetCore.Examples.ProductService.Requests;

namespace AspNetCore.Examples.ProductService.GetProductById
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