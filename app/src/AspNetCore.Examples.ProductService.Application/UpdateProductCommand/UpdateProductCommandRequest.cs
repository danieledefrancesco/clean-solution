using AspNetCore.Examples.ProductService.Products;
using AspNetCore.Examples.ProductService.Requests;

namespace AspNetCore.Examples.ProductService.UpdateProductCommand
{
    public sealed class UpdateProductCommandRequest: IAppRequest<UpdateProductCommandResponse>
    {
        public UpdateProductCommandRequest(ProductId productId, ProductName productName, ProductPrice productPrice)
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