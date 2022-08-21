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
        private HttpClient _httpClient;

        [SetUp]
        public void SetUp()
        {
            _options = Substitute.For<IOptions<PriceCardServiceClientConfiguration>>();
            _httpClient = Substitute.For<HttpClient>();
            
            _priceCardServiceClientConfiguration = new PriceCardServiceClientConfiguration();
            _options.Value.Returns(_priceCardServiceClientConfiguration);
            
            _priceCardServiceClientFactory = new PriceCardServiceClientFactory(_options, _httpClient);
        }

        [Test]
        public void Create_ReturnsPriceCardServiceClient()
        {
            _priceCardServiceClientConfiguration.PriceCardServiceBaseUri = "https://test.com/";

            var priceCardServiceClient = _priceCardServiceClientFactory.Create();

            priceCardServiceClient.Should().NotBeNull();
            priceCardServiceClient.BaseUrl.Should().Be(_priceCardServiceClientConfiguration.PriceCardServiceBaseUri);

        }
    }
}