using AspNetCore.Examples.ProductService.Products;

namespace AspNetCore.Examples.ProductService.GetProductById
{
    public sealed class GetProductByIdResponse
    {
        public GetProductByIdResponse(Product product)
        {
            Product = product;
        }

        public Product Product { get; }
    }
}