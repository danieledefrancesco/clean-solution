using System.Net.Http;
using FluentAssertions;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.Factories
{
    public class DefaultHttpClientFactoryTest
    {
        private DefaultHttpClientFactory _defaultHttpClientFactory;
        [SetUp]
        public void SetUp()
        {
            _defaultHttpClientFactory = new DefaultHttpClientFactory();
        }

        [Test]
        public void CreateClient_ReturnsHttpClient()
        {
            _defaultHttpClientFactory.CreateClient().Should().BeOfType<HttpClient>();
        }
    }
}