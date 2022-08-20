using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Common;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.Repositories
{
    public abstract class RepositoryTestBase<TEntity, TId>
        where TEntity : EntityBase<TId>
        where TId : class
    {
        protected RepositoryBase<TEntity, TId> _repository;
        protected DbContext _dbContext;
        protected DbSet<TEntity> _dbSet;

        [SetUp]
        public virtual void SetUp()
        {
            _dbContext = CreateDbContext();
            _repository = CreteRepository(_dbContext);
            _dbSet = _dbContext.Set<TEntity>();
            _dbSet.RemoveRange(_dbSet.ToList());
        }

        protected abstract RepositoryBase<TEntity, TId> CreteRepository(
            DbContext dbContext);

        protected abstract DbContext CreateDbContext();

        [Test]
        public virtual async Task ExistsById_ReturnsTrue_IfEntityExists()
        {
            var entity = CreateTestEntity();
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            (await _repository.ExistsById(entity.Id))
                .Should()
                .BeTrue();
        }

        [Test]
        public virtual async Task ExistsById_ReturnsFalse_IfEntityDoesntExist()
        {
            var id = CreateId();

            (await _repository.ExistsById(id))
                .Should()
                .BeFalse();
        }

        [Test]
        public virtual async Task GetById_ReturnsEntity_IfEntityExists()
        {
            var entity = CreateTestEntity();
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            (await _repository.GetById(entity.Id))
                .Should()
                .Be(entity);
        }

        [Test]
        public virtual async Task GetById_ReturnsNull_IfEntityDoesntExist()
        {
            var id = CreateId();

            (await _repository.GetById(id))
                .Should()
                .BeNull();
        }

        [Test]
        public virtual async Task Delete_DeletesEntity()
        {
            var entityToDelete = CreateTestEntity();

            await _dbSet.AddAsync(entityToDelete);
            await _dbContext.SaveChangesAsync();

            await _repository
                .Delete(entityToDelete);
            
            await _dbContext.SaveChangesAsync();

            (await _dbSet.CountAsync())
                .Should()
                .Be(0);
        }

        [Test]
        public virtual async Task DeleteById_DeletesEntity()
        {
            var entityToDelete = CreateTestEntity();

            await _dbSet.AddAsync(entityToDelete);
            await _dbContext.SaveChangesAsync();

            await _repository
                .DeleteById(entityToDelete.Id);
            await _dbContext.SaveChangesAsync();

            (await _dbSet.CountAsync())
                .Should()
                .Be(0);
        }

        [Test]
        public virtual async Task Insert_InsertsEntityAndSetsCreatedAtAndLastModifiedAt()
        {
            var entityToInsert = CreateTestEntity();

            await _repository
                .Insert(entityToInsert);
            await _dbContext.SaveChangesAsync();


            (await _dbSet.CountAsync())
                .Should()
                .Be(1);

            entityToInsert
                .CreatedAt
                .Should()
                .NotBe(default);

            entityToInsert
                .LastModifiedAt
                .Should()
                .Be(entityToInsert.CreatedAt);
        }

        [Test]
        public virtual async Task Update_UpdatesEntityAndSetsLastModifiedAt()
        {
            var entityToUpdate = CreateTestEntity();

            await _dbSet.AddAsync(entityToUpdate);
            await _dbContext.SaveChangesAsync();

            await _repository
                .Update(entityToUpdate);
            
            await _dbContext.SaveChangesAsync();

            entityToUpdate
                .LastModifiedAt
                .Should()
                .NotBe(default);
        }


        protected abstract TEntity CreateTestEntity();
        protected abstract TId CreateId();
    }
}