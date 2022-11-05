using AspNetCore.Examples.ProductService.Entities;

namespace AspNetCore.Examples.ProductService.Responses
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