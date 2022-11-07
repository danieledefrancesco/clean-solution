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

        [Post("/products")]
        public Task<Response<ProductDto>> CreatePost([Body] CreateProductRequestDto createProductRequest);
        
        [Put("/products")]
        public Task<Response<ProductDto>> CreatePut([Body] CreateProductRequestDto createProductRequest);
    }
}