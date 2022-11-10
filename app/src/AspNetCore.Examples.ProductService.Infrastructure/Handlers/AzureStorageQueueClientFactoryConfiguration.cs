namespace AspNetCore.Examples.ProductService.Handlers
{
    public sealed class AzureStorageQueueClientFactoryConfiguration
    {
        public AzureStorageQueueClientFactoryConfiguration(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; }
    }
}