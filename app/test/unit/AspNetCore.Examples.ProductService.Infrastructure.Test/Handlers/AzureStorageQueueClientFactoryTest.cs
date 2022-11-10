using FluentAssertions;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.Handlers
{
    public sealed class AzureStorageQueueClientFactoryTest
    {
        private AzureStorageQueueClientFactory<TestDomainEvent,TestDomainEventDto> _azureStorageQueueClientFactory;
        private AzureStorageQueueClientFactoryConfiguration _configuration;
        
        [SetUp]
        public void SetUp()
        {
            var connectionString =
                "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;QueueEndpoint=http://queues:10001/devstoreaccount1";
            _configuration = new AzureStorageQueueClientFactoryConfiguration(connectionString);
            _azureStorageQueueClientFactory =
                new AzureStorageQueueClientFactory<TestDomainEvent, TestDomainEventDto>(_configuration);
        }

        [Test]
        public void Create_ReturnsNonNullQueueClient()
        {
            _azureStorageQueueClientFactory.Create().Should().NotBeNull();
        }
    }
}