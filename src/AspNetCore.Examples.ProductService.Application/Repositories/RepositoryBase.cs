using System;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Common;
using AspNetCore.Examples.ProductService.Persistence;

namespace AspNetCore.Examples.ProductService.Repositories
{
    public abstract class RepositoryBase<TEntity, TId> : IRepository<TEntity, TId>
    where TEntity : EntityBase<TId>
    where TId : class
    {
        private readonly IPersistenceImplementation<TEntity,TId> _persistenceImplementation;

        protected RepositoryBase(IPersistenceImplementation<TEntity, TId> persistenceImplementation)
        {
            _persistenceImplementation = persistenceImplementation;
        }

        public async Task<bool> ExistsById(TId id)
        {
            var currentEntity = await GetById(id);
            return currentEntity != null;
        }

        public Task<TEntity> GetById(TId id) => _persistenceImplementation.GetById(id);

        public Task DeleteById(TId id) => _persistenceImplementation.DeleteById(id);

        public Task Delete(TEntity entity) => _persistenceImplementation.Delete(entity);

        public Task Insert(TEntity entity)
        {
            entity.CreatedAt = DateTime.Now;
            entity.LastModifiedAt = entity.CreatedAt;
            return _persistenceImplementation.Insert(entity);
        }

        public Task Update(TEntity entity)
        {
            entity.LastModifiedAt = DateTime.Now;
            return _persistenceImplementation.Update(entity);
        }
    }
}