using System.Net.Http;
using AspNetCore.Examples.ProductService.Entities;
using RestEase;

namespace AspNetCore.Examples.ProductService.Specs
{
    public static class TestData
    {
        private static Response<ProductDto> _productResponse;
        private static ApiException _apiError;
        
        public static int HttpStatusCode { get; private set; }

        public static Response<ProductDto> ProductResponse
        {
            get => _productResponse;
            set
            {
                _productResponse = value;
                HttpStatusCode = (int?) value?.ResponseMessage.StatusCode ?? 0;
            }
        }
        public static ApiException ApiError
        {
            get => _apiError;
            set
            {
                _apiError = value;
                HttpStatusCode = (int?) value?.StatusCode ?? 0;
            }
        }

        public static CreateProductRequestDto CreateProductRequest { get; set; }

        public static void Reset()
        {
            ProductResponse = null;
            ApiError = null;
            CreateProductRequest = null;
        }
    }
}