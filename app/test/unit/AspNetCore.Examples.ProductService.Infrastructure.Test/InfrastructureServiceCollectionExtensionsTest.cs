using System;
using System.Collections.Generic;
using System.Net.Http;
using AspNetCore.Examples.ProductService.Factories;
using AspNetCore.Examples.ProductService.Persistence;
using AspNetCore.Examples.ProductService.Persistence.ClassMapRegistrationProviders;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService
{
    public class InfrastructureServiceCollectionExtensionsTest
    {
        private IServiceCollection _services;
        private IConfiguration _configuration;
        private IServiceProvider _serviceProvider;
        private IMongoDbClassMapsRegister _mongoDbClassMapsRegister;

        [SetUp]
        public void SetUp()
        {
            _mongoDbClassMapsRegister = Substitute.For<IMongoDbClassMapsRegister>();
            _mongoDbClassMapsRegister
                .When(x => x.RegisterAllClassMaps())
                .Do(x => { });
            
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>()
                {
                    {$"{nameof(MongoDbConfiguration)}:{nameof(MongoDbConfiguration.DatabaseName)}","DatabaseName"},
                    {$"{nameof(MongoDbConfiguration)}:{nameof(MongoDbConfiguration.ConnectionString)}","ConnectionString"}
                })
            .Build();

            _services = new ServiceCollection();
            _services.AddMongoDb(_configuration, _mongoDbClassMapsRegister);
            _services.AddDefaultHttpClientFactory();
            _serviceProvider = _services.BuildServiceProvider();
        }

        [Test]
        public void AddMongoDb_SetsConfigurationCorrectly()
        {
            var config = _serviceProvider.GetRequiredService<IOptions<MongoDbConfiguration>>().Value;

            config
                .DatabaseName
                .Should()
                .Be("DatabaseName");

            config
                .ConnectionString
                .Should()
                .Be("ConnectionString");
        }

        [Test]
        public void AddMongoDb_AddsMongoDbProvider()
        {
            _serviceProvider
                .GetRequiredService<IMongoDbProvider>()
                .Should()
                .NotBeNull();
        }

        [Test]
        public void AddMongoDb_AddsMongoDbPersistenceImplementation()
        {
            _services
                .Should()
                .Contain(x =>
                    x.ServiceType == typeof(IPersistenceImplementation<,>) &&
                    x.ImplementationType == typeof(MongoDbPersistenceImplementation<,>));
        }

        [Test]
        public void AddDefaultHttpClientFactory_AddsDefaultHttpClientFactoryConfiguration()
        {
            _services.Should().Contain(x =>
                x.ServiceType == typeof(IHttpClientFactory) &&
                x.ImplementationType == typeof(DefaultHttpClientFactory));
        }
    }
}