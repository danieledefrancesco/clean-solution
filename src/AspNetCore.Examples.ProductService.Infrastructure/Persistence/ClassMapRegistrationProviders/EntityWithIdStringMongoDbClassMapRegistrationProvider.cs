using AspNetCore.Examples.ProductService.Common;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;

namespace AspNetCore.Examples.ProductService.Persistence.ClassMapRegistrationProviders
{
    public class EntityWithIdStringMongoDbClassMapRegistrationProvider
        : MongoDbClassMapRegistrationProviderBase<EntityBase<string>>
    {
        protected override void InitializeClassMap(BsonClassMap<EntityBase<string>> classMap)
        {
            classMap.AutoMap();
            classMap.MapIdField(p => p.Id)
                .SetIdGenerator(StringObjectIdGenerator.Instance);
        }
    }
}