using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Newtonsoft.Json;

namespace AspNetCore.Examples.ProductService.Handlers
{
    public sealed class AzureStorageQueueHandler<T> : IQueueHandler<T>
    {
        private readonly QueueClient _queueClient;

        public AzureStorageQueueHandler(QueueClient queueClient)
        {
            _queueClient = queueClient;
        }

        private string SerializeEvent(T @event)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
            return JsonConvert.SerializeObject(@event, settings);
        }

        public async Task SendMessageAsync(T @event, CancellationToken token)
        {
            await _queueClient.SendMessageAsync(SerializeEvent(@event), token);
        }
    }
}