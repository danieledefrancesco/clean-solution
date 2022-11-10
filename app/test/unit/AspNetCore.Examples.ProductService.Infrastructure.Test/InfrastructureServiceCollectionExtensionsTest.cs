using System;
using System.Net.Http;
using AspNetCore.Examples.PriceCardService;
using AspNetCore.Examples.ProductService.Configurations;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService
{
    public sealed class InfrastructureServiceCollectionExtensionsTest
    {
        [Test]
        public void AddInfrastructureLayer_ShouldNotThrow()
        {
            var services = new ServiceCollection();
            var configuration = Substitute.For<IConfiguration>();
            var action = (Action)(() => services.AddInfrastructureLayer(configuration));
            action.Should().NotThrow();
        }

        [Test]
        public void ConfigureHttpClientForPriceCardServiceClosure_SetsBaseUri()
        {
            var httpClient = new HttpClient();
            const string priceCardServiceBaseUri = "https://testuri.com";
            var config = new PriceCardServiceClientConfiguration
            {
                PriceCardServiceBaseUri = priceCardServiceBaseUri
            };
            var sp = Substitute.For<IServiceProvider>();
            sp.GetService(typeof(HttpClient)).Returns(httpClient);
            var factory =
                InfrastructureServiceCollectionExtensions.CreatePriceCardServiceClientClosure(config, typeof(PriceCardServiceClient));
            var configuredPriceCardServiceClient = (PriceCardServiceClient)factory(sp);
            configuredPriceCardServiceClient.BaseUrl.Should().Be(priceCardServiceBaseUri);
        }
    }
}