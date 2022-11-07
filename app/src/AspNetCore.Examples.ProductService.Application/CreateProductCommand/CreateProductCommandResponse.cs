using AspNetCore.Examples.ProductService.Products;

namespace AspNetCore.Examples.ProductService.CreateProductCommand
{
    public sealed class CreateProductCommandResponse
    {
        public Product CreatedProduct { get; }

        public CreateProductCommandResponse(Product createdProduct)
        {
            CreatedProduct = createdProduct;
        }
    }
}