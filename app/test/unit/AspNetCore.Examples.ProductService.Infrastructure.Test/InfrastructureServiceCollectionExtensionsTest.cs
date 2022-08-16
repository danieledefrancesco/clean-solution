using System;
using System.Net.Http;
using AspNetCore.Examples.ProductService.Factories;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService
{
    public class InfrastructureServiceCollectionExtensionsTest
    {
        private IServiceCollection _services;
        private IServiceProvider _serviceProvider;
        private string _oldVariableValue;
        private string _connectionStringEnvironmentVariableName = "DATABASE_CONNECTION_STRING";

        [SetUp]
        public void SetUp()
        {
            _services = new ServiceCollection();
            _services.AddEntityFrameworkForSqlServer();
            _services.AddDefaultHttpClientFactory();
            _serviceProvider = _services.BuildServiceProvider();
            _oldVariableValue = Environment.GetEnvironmentVariable(_connectionStringEnvironmentVariableName);
            Environment.SetEnvironmentVariable(_connectionStringEnvironmentVariableName, "ConnectionString");
        }

        [TearDown]
        public void TearDown()
        {
            Environment.SetEnvironmentVariable(_connectionStringEnvironmentVariableName, _oldVariableValue);
        }

        [Test]
        public void AddEntityFrameworkForSqlServer_AddsEntityFramework()
        {
            _serviceProvider
                .GetRequiredService<AppDbContext>()
                .Should()
                .NotBeNull();
            
            _serviceProvider
                .GetRequiredService<AppDbContext>()
                .Should()
                .NotBeNull();
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