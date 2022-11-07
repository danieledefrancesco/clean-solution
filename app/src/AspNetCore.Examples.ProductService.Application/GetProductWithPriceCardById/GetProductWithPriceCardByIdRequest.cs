using AspNetCore.Examples.ProductService.Products;
using AspNetCore.Examples.ProductService.Requests;

namespace AspNetCore.Examples.ProductService.GetProductWithPriceCardById
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