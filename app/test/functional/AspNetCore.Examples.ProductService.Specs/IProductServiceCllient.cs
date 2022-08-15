using System.Threading.Tasks;
using RestEase;

namespace AspNetCore.Examples.ProductService.Specs
{
    
    public interface IProductServiceClient
    {
        [Get("/products/{id}")]
        public Task<Response<ProductDto>> GetById([Path("id")]string id);

        [Post("/products")]
        public Task<Response<ProductDto>> Create([Body] CreateProductRequestDto createProductRequest);
    }
}