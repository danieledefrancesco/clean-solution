using System.Linq;
using System.Threading.Tasks;
using Catalog.Domain.Entities;

namespace Catalog.Infrastructure.Abstractions.DataAccessLayer
{
    public interface IDataAccessObject<EntityType,in IdType> where EntityType : Entity<IdType>
    {
        Task<IQueryable<EntityType>> GetAllAsync();
        Task<EntityType> GetByIdAsync(IdType id);
        Task<EntityType> AddAsync(EntityType entity);
        Task<EntityType> UpdateAsync(EntityType entity);
        Task<EntityType> DeleteAsync(EntityType entity);
        Task SaveChangesAsync();
    }
}