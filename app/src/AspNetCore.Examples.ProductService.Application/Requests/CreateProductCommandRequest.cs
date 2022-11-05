using AspNetCore.Examples.ProductService.Responses;
using AspNetCore.Examples.ProductService.ValueObjects;

namespace AspNetCore.Examples.ProductService.Requests
{
    public sealed class CreateProductCommandRequest : IAppRequest<CreateProductCommandResponse>
    {
        public CreateProductCommandRequest(ProductId productId, ProductName productName, ProductPrice productPrice)
        {
            ProductId = productId;
            ProductName = productName;
            ProductPrice = productPrice;
        }

        public ProductId ProductId { get; }
        public ProductName ProductName { get; }
        public ProductPrice ProductPrice { get; }
    }
}