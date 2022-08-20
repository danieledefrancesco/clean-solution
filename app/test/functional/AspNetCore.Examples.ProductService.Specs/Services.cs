using System;
using AspNetCore.Examples.ProductService.Events;
using Azure.Storage.Queues;
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
        static Services()
        {
            Reset();
        }
        public static void Reset()
        {
            AppDbContext = new AppDbContext();
            ProductServiceClient = RestEase.RestClient.For<IProductServiceClient>(Environment.GetEnvironmentVariable("SUT_BASE_URL")!);
            WiremockAdminClient = new RestEase.RestClient(Environment.GetEnvironmentVariable("WIREMOCK_BASE_URL")!)
            {
                JsonSerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                }
            }.For<IWiremockAdminClient>();
            OnProductCreatedEventQueueClient = new QueueClient(Environment.GetEnvironmentVariable("QUEUE_STORAGE_CONNECTION_STRING"),
                nameof(OnProductCreated).ToLower());
            OnProductCreatedEventQueueClient.DeleteIfExists();
            OnProductCreatedEventQueueClient.CreateIfNotExists();
        }
    }
}