using AspNetCore.Examples.ProductService.Entities;
using AspNetCore.Examples.ProductService.Responses;

namespace AspNetCore.Examples.ProductService.Requests
{
    public class CreateProductCommandRequest : IAppRequest<CreateProductCommandResponse>
    {
        public Product ProductToCreate { get; }

        public CreateProductCommandRequest(Product productToCreate)
        {
            ProductToCreate = productToCreate;
        }
    }
}