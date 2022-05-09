using System;
using System.Net.Http;
using AspNetCore.Examples.PriceCardService;
using AspNetCore.Examples.ProductService.Configurations;
using Microsoft.Extensions.Options;

namespace AspNetCore.Examples.ProductService.Factories
{
    public class PriceCardServiceClientFactory: IPriceCardServiceClientFactory
    {
        private readonly IOptions<PriceCardServiceClientConfiguration> _config;
        private readonly IHttpClientFactory _httpClientFactory;

        public PriceCardServiceClientFactory(IOptions<PriceCardServiceClientConfiguration> config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        public PriceCardServiceClient Create()
        {
            var client = new PriceCardServiceClient(_httpClientFactory.CreateClient())
            {
                BaseUrl = _config.Value.PriceCardServiceBaseUri
            };
            return client;
        }
    }
}