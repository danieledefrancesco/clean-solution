using System;
using AspNetCore.Examples.ProductService.Products;
using Azure.Storage.Queues;
using Microsoft.Extensions.Http.Logging;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AspNetCore.Examples.ProductService.Specs
{
    public static class Services
    {
        public static AppDbContext AppDbContext { get; private set; }
        public static IProductServiceClient ProductServiceClient { get; private set; }
        public static IWiremockAdminClient WiremockAdminClient { get; private set; }

        public static QueueClient OnProductCreatedEventQueueClient { get; private set; }
        public static QueueClient OnProductUpdatedEventQueueClient { get; private set; }
        static Services()
        {
            Reset();
        }
        public static void Reset()
        {
            AppDbContext = new AppDbContext();
            ProductServiceClient = new RestEase.RestClient(Environment.GetEnvironmentVariable("SUT_BASE_URL")!)
                .For<IProductServiceClient>();
            WiremockAdminClient = new RestEase.RestClient(Environment.GetEnvironmentVariable("WIREMOCK_BASE_URL")!)
            {
                JsonSerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                }
            }.For<IWiremockAdminClient>();
            OnProductCreatedEventQueueClient = CreateClientForQueue<OnProductCreated>();
            OnProductUpdatedEventQueueClient = CreateClientForQueue<OnProductUpdated>();
        }
        
        private static QueueClient CreateClientForQueue<T>()
        {
            var result = new QueueClient(Environment.GetEnvironmentVariable("QUEUE_STORAGE_CONNECTION_STRING"),
                typeof(T).Name.ToLower());
            result.DeleteIfExists();
            result.CreateIfNotExists();
            return result;
        }
    }
}