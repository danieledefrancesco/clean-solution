using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Common;
using MongoDB.Driver;

namespace AspNetCore.Examples.ProductService.Persistence
{
    public class MongoDbPersistenceImplementation<TEntity, TId> : IPersistenceImplementation<TEntity, TId> 
        where TEntity : EntityBase<TId>
        where TId : class
    {
        private readonly IMongoDbProvider _mongoDbProvider;
        private IMongoDatabase _mongoDatabase;
        private IMongoCollection<TEntity> _rootCollection;

        public MongoDbPersistenceImplementation(IMongoDbProvider mongoDbProvider)
        {
            _mongoDbProvider = mongoDbProvider;
        }

        private IMongoDatabase Database => _mongoDatabase ??= _mongoDbProvider.GetDatabase();

        public IMongoCollection<TEntity> RootCollection =>
            _rootCollection ??= Database.GetCollection<TEntity>(typeof(TEntity).Name.ToLower());


        public Task<TEntity> GetById(TId id)
        {
            return RootCollection.Find(entity => entity.Id == id).SingleOrDefaultAsync();
        }

        public Task DeleteById(TId id)
        {
            return RootCollection.DeleteOneAsync(entity => entity.Id == id);
        }

        public Task Delete(TEntity entity)
        {
            return RootCollection.DeleteOneAsync(entity2 => entity2.Id == entity.Id);
        }

        public Task Insert(TEntity entity)
        {
            return RootCollection.InsertOneAsync(entity);
        }

        public Task Update(TEntity entity)
        {
            return RootCollection.ReplaceOneAsync(entity2 => entity2.Id == entity.Id, entity);
        }
    }
}