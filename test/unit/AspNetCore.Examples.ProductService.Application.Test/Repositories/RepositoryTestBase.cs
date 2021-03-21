using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Common;
using AspNetCore.Examples.ProductService.Persistence;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.Repositories
{
    public abstract class RepositoryTestBase<TEntity, TId>
    where TEntity : EntityBase<TId>
    where TId : class
    {
        protected RepositoryBase<TEntity, TId> _repository;
        protected IPersistenceImplementation<TEntity, TId> _persistenceImplementation;

        [SetUp]
        public virtual void SetUp()
        {
            _persistenceImplementation = Substitute.For<IPersistenceImplementation<TEntity,TId>>();
            _repository = CreteRepository(_persistenceImplementation);
        }

        protected abstract RepositoryBase<TEntity, TId> CreteRepository(
            IPersistenceImplementation<TEntity, TId> persistenceImplementation);

        [Test]
        public virtual void Test_GetEntityById_ReturnsEntity_IfEntityExists()
        {
            var entity = CreateTestEntity();
            var persistenceImplementationResult = Task.FromResult(entity);
            _persistenceImplementation.GetById(entity.Id).Returns(persistenceImplementationResult);

            _persistenceImplementation.GetById(entity.Id)
                .Result
                .Should()
                .Be(entity);
        }

        protected abstract TEntity CreateTestEntity();
    }
}