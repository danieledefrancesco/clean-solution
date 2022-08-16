using System.Threading.Tasks;
using RestEase;

namespace AspNetCore.Examples.ProductService.Specs
{
    
    public interface IProductServiceClient
    {
        [Get("/products/{id}")]
        public Task<Response<ProductDto>> GetById([Path("id")]string id);

        [Post("/products")]
        public Task<Response<ProductDto>> CreatePost([Body] CreateProductRequestDto createProductRequest);
        [Put("/products")]
        public Task<Response<ProductDto>> CreatePut([Body] CreateProductRequestDto createProductRequest);
    }
}