using Azure.Storage.Queues;

namespace AspNetCore.Examples.ProductService.Handlers
{
    public interface IAzureStorageQueueClientFactory<T>
    {
        QueueClient Create();
    }
}