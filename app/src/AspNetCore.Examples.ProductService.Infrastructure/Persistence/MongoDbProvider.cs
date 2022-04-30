using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AspNetCore.Examples.ProductService.Persistence
{
    public class MongoDbProvider : IMongoDbProvider
    {
        private readonly IOptions<MongoDbConfiguration> _mongoDbConfiguration;

        public MongoDbProvider(IOptions<MongoDbConfiguration> mongoDbConfiguration)
        {
            _mongoDbConfiguration = mongoDbConfiguration;
        }

        private IMongoClient GetClient()
        {
            return new MongoClient(_mongoDbConfiguration.Value.ConnectionString);
        }
        
        public IMongoDatabase GetDatabase()
        {
            var client = GetClient();
            return client.GetDatabase(_mongoDbConfiguration.Value.DatabaseName);
        }
    }

}