using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Entities;

namespace AspNetCore.Examples.ProductService.Repositories
{
    public interface IRepository<TEntity, in TId> 
        where TEntity : IAggregateRoot<TId>
        where TId : class
    {
        Task<bool> ExistsById(TId id);
        Task<TEntity> GetById(TId id);
        Task DeleteById(TId id);
        Task Delete(TEntity entity);
        Task<TEntity> Insert(TEntity entity);
        Task Update(TEntity entity);
        
    }
}