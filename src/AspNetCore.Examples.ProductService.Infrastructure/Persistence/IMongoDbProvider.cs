using MongoDB.Driver;

namespace AspNetCore.Examples.ProductService.Persistence
{
    public interface IMongoDbProvider
    {
        IMongoDatabase GetDatabase();
    }
}