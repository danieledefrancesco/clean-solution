using System.Threading.Tasks;
using RestEase;

namespace AspNetCore.Examples.ProductService.Specs
{
    
    public interface IProductServiceClient
    {
        [Get("/products/{id}/with-price-card")]
        public Task<Response<ProductWithPriceCardDto>> GetWithPriceCardById([Path("id")]string id);
        
        [Get("/products/{id}")]
        public Task<Response<ProductDto>> GetById([Path("id")]string id);
        
        [Patch("/products/{id}")]
        public Task<Response<ProductDto>> Update([Path("id")]string id, [Body]UpdateProductCommandRequestDtoBody body);

        [Post("/products")]
        public Task<Response<ProductDto>> CreatePost([Body] CreateProductCommandRequestDto createProductRequest);
        
        [Put("/products")]
        public Task<Response<ProductDto>> CreatePut([Body] CreateProductCommandRequestDto createProductRequest);
    }
}