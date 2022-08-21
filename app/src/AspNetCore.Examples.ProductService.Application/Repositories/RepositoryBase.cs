using System;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Common;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Examples.ProductService.Repositories
{
    public abstract class RepositoryBase<TEntity, TId> : IRepository<TEntity, TId>
    where TEntity : EntityBase<TId>
    where TId : class
    {
        protected DbContext DbContext { get; }
        protected DbSet<TEntity> Set { get; }

        protected RepositoryBase(DbContext dbContext)
        {
            DbContext = dbContext;
            Set = DbContext.Set<TEntity>();
        }


        public async Task<bool> ExistsById(TId id)
        {
            var currentEntity = await GetById(id);
            return currentEntity != null;
        }

        public Task<TEntity> GetById(TId id) => Set.FirstOrDefaultAsync(x => x.Id == id);

        public async Task DeleteById(TId id)
        {
            var entity = await GetById(id);
            await Delete(entity);
        }

        public Task Delete(TEntity entity)
        {
            Set.Remove(entity);
            return Task.CompletedTask;
        } 

        public async Task<TEntity> Insert(TEntity entity)
        {
            entity.CreatedAt = DateTime.Now;
            entity.LastModifiedAt = entity.CreatedAt;
            var addResult = await Set.AddAsync(entity);
            return addResult.Entity;
        }

        public Task Update(TEntity entity)
        {
            entity.LastModifiedAt = DateTime.Now;
            Set.Update(entity);
            return Task.CompletedTask;
        }
    }
}