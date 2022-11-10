using Azure.Storage.Queues;

namespace AspNetCore.Examples.ProductService.Handlers
{
    public sealed class AzureStorageQueueClientFactory<TDomainEvent, TEventDto> : IAzureStorageQueueClientFactory<TEventDto>
    {
        private readonly AzureStorageQueueClientFactoryConfiguration _configuration;

        public AzureStorageQueueClientFactory(AzureStorageQueueClientFactoryConfiguration configuration)
        {
            _configuration = configuration;
        }

        public QueueClient Create() => new (_configuration.ConnectionString, typeof(TDomainEvent)!.Name!.ToLower());
        
    }
}