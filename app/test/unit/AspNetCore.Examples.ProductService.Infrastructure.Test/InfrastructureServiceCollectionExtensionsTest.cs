using System;
using System.Net.Http;
using AspNetCore.Examples.PriceCardService;
using AspNetCore.Examples.ProductService.Factories;
using AspNetCore.Examples.ProductService.Handlers;
using Azure.Storage.Queues;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using ZapMicro.TransactionalOutbox.Commands;

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
            var configuration = Substitute.For<IConfiguration>();
            configuration["QUEUE_STORAGE_CONNECTION_STRING"].Returns("");
            _services = new ServiceCollection();
            _services.AddEntityFrameworkForSqlServer();
            _services.AddDefaultHttpClientFactory();
            _services.AddPriceCardService();
            _services.AddTransactionalOutbox();
            _services.AddAzureStorageQueues(configuration);
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
        public void AddPriceCardService_AddsClientAndFactory()
        {
            _services.Should().Contain(x =>
                x.ServiceType == typeof(PriceCardServiceClient));
            _services.Should().Contain(x =>
                x.ServiceType == typeof(IPriceCardServiceClientFactory));
        }

        [Test]
        public void AddTransactionalOutbox_AddsTransactionalOutboxServices()
        {
            _services.Should().Contain(x => x.ServiceType == typeof(IEnqueueOutboxMessageCommand));
            _services.Should().Contain(x =>
                x.ServiceType == typeof(IEventHandler) &&
                x.ImplementationType == typeof(TransactionalOutboxEventHandler));
        }

        [Test]
        public void AddAzureStorageQueue_AddsQueueClient()
        {
            _services.Should().Contain(x =>
                x.ServiceType == typeof(Func<QueueClient>));
        }
    }
}