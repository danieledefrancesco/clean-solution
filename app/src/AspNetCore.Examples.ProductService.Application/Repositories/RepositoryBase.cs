using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Examples.ProductService.Repositories
{
    public abstract class RepositoryBase<TEntity, TId> : IRepository<TEntity, TId>
    where TEntity : class, IAggregateRoot<TId>
    where TId : class
    {
        private DbContext DbContext { get; }
        private DbSet<TEntity> Set { get; }

        protected RepositoryBase(DbContext dbContext)
        {
            DbContext = dbContext;
            Set = DbContext.Set<TEntity>();
        }


        public virtual async Task<bool> ExistsById(TId id)
        {
            var currentEntity = await GetById(id);
            return currentEntity != null;
        }

        public virtual Task<TEntity> GetById(TId id) => Set.FirstOrDefaultAsync(x => x.Id == id);

        public virtual async Task DeleteById(TId id)
        {
            var entity = await GetById(id);
            await Delete(entity);
        }

        public virtual Task Delete(TEntity entity)
        {
            Set.Remove(entity);
            return Task.CompletedTask;
        } 

        public virtual async Task<TEntity> Insert(TEntity entity)
        {
            var addResult = await Set.AddAsync(entity);
            return addResult.Entity;
        }

        public virtual Task Update(TEntity entity)
        {
            Set.Update(entity);
            return Task.CompletedTask;
        }
    }
}