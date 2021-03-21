using MongoDB.Bson.Serialization;

namespace AspNetCore.Examples.ProductService.Persistence.ClassMapRegistrationProviders
{
    public abstract class MongoDbClassMapRegistrationProviderBase<T> : IMongoDbClassMapRegistrationProvider<T>
    where T  : class
    {
        public void RegisterClassMap()
        {
            BsonClassMap.RegisterClassMap<T>(InitializeClassMap);
        }

        protected abstract void InitializeClassMap(BsonClassMap<T> classMap);
    }
}