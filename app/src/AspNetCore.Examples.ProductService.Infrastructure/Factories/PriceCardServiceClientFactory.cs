using System.Net.Http;
using AspNetCore.Examples.PriceCardService;
using AspNetCore.Examples.ProductService.Configurations;
using Microsoft.Extensions.Options;

namespace AspNetCore.Examples.ProductService.Factories
{
    public sealed class PriceCardServiceClientFactory: IPriceCardServiceClientFactory
    {
        private readonly IOptions<PriceCardServiceClientConfiguration> _config;
        private readonly HttpClient _httpClient;

        public PriceCardServiceClientFactory(IOptions<PriceCardServiceClientConfiguration> config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
        }

        public PriceCardServiceClient Create()
        {
            var client = new PriceCardServiceClient(_httpClient)
            {
                BaseUrl = _config.Value.PriceCardServiceBaseUri
            };
            return client;
        }
    }
}