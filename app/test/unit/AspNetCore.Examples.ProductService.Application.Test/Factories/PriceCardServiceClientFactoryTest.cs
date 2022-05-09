using System.Net.Http;
using AspNetCore.Examples.ProductService.Configurations;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.Factories
{
    public class PriceCardServiceClientFactoryTest
    {
        private PriceCardServiceClientFactory _priceCardServiceClientFactory;
        private IOptions<PriceCardServiceClientConfiguration> _options;
        private PriceCardServiceClientConfiguration _priceCardServiceClientConfiguration;
        private IHttpClientFactory _httpClientFactory;

        [SetUp]
        public void SetUp()
        {
            _options = Substitute.For<IOptions<PriceCardServiceClientConfiguration>>();
            _httpClientFactory = Substitute.For<IHttpClientFactory>();
            
            _priceCardServiceClientConfiguration = new PriceCardServiceClientConfiguration();
            _options.Value.Returns(_priceCardServiceClientConfiguration);
            
            _priceCardServiceClientFactory = new PriceCardServiceClientFactory(_options, _httpClientFactory);
        }

        [Test]
        public void Create_ReturnsPriceCardServiceClient()
        {
            var httpClient = Substitute.For<HttpClient>();
            _httpClientFactory.CreateClient().Returns(httpClient);
            
            _priceCardServiceClientConfiguration.PriceCardServiceBaseUri = "https://test.com/";

            var priceCardServiceClient = _priceCardServiceClientFactory.Create();

            priceCardServiceClient.Should().NotBeNull();
            priceCardServiceClient.BaseUrl.Should().Be(_priceCardServiceClientConfiguration.PriceCardServiceBaseUri);

            _httpClientFactory.CreateClient().Received(1);
        }
    }
}