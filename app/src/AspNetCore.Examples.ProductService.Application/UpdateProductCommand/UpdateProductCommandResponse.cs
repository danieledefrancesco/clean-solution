using AspNetCore.Examples.ProductService.Products;

namespace AspNetCore.Examples.ProductService.UpdateProductCommand
{
    public sealed class UpdateProductCommandResponse
    {
        public UpdateProductCommandResponse(Product updatedProduct)
        {
            UpdatedProduct = updatedProduct;
        }

        public Product UpdatedProduct { get; }
    }
}