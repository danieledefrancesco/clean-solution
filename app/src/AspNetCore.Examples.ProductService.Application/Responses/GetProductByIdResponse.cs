using AspNetCore.Examples.ProductService.Entities;

namespace AspNetCore.Examples.ProductService.Responses
{
    public class GetProductByIdResponse
    {
        public GetProductByIdResponse(Product product)
        {
            Product = product;
        }

        public Product Product { get; }
    }
}