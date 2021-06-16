using System;
using AspNetCore.Examples.ProductService.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService
{
    public class ApplicationServiceCollectionExtensionsTest
    {
        private IServiceCollection _services;
        private IConfiguration _configuration;
        [SetUp]
        public void Setup()
        {
            _configuration = new ConfigurationBuilder()
                .Build();
            _services = new ServiceCollection().AddApplicationServices(_configuration);
        }

        [Test]
        public void Test_AddApplicationServices_AddsProductRepository()
        {
            _services
                .Should()
                .ContainSingle(x =>
                    x.ServiceType == typeof(IProductRepository)
                    &&
                    x.ImplementationType == typeof(ProductRepository));

        }
    }
}