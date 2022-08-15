using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AspNetCore.Examples.ProductService.Specs
{
    public class Services
    {
        public static AppDbContext AppDbContext { get; private set; }
        public static IProductServiceClient ProductServiceClient { get; private set; }
        public static IWiremockAdminClient WiremockAdminClient { get; private set; }

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
        }
    }
}