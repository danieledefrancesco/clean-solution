using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Common;

namespace AspNetCore.Examples.ProductService.Persistence
{
    public interface IPersistenceImplementation<TEntity, in TId>
        where TEntity : EntityBase<TId>
        where TId : class
    {
        Task<TEntity> GetById(TId id);
        Task DeleteById(TId id);
        Task Delete(TEntity entity);
        Task Insert(TEntity entity);
        Task Update(TEntity entity);

    }
}