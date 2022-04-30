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
        public virtual void ExistsById_ReturnsTrue_IfEntityExists()
        {
            var entity = CreateTestEntity();
            var persistenceImplementationResult = Task.FromResult(entity);
            _persistenceImplementation.GetById(entity.Id).Returns(persistenceImplementationResult);

            _repository.ExistsById(entity.Id)
                .Result
                .Should()
                .BeTrue();
        }

        [Test]
        public virtual void ExistsById_ReturnsFalse_IfEntityDoesntExist()
        {
            var id = CreateId();
            TEntity entity = null;
            _persistenceImplementation.GetById(id).Returns(entity);

            _repository.ExistsById(id)
                .Result
                .Should()
                .BeFalse();
        }

        [Test]
        public virtual void GetById_ReturnsEntity_IfEntityExists()
        {
            var entity = CreateTestEntity();
            var persistenceImplementationResult = Task.FromResult(entity);
            _persistenceImplementation.GetById(entity.Id).Returns(persistenceImplementationResult);

            _repository.GetById(entity.Id)
                .Result
                .Should()
                .Be(entity);
        }

        [Test]
        public virtual void GetById_ReturnsNull_IfEntityDoesntExist()
        {
            var id = CreateId();
            TEntity entity = null;
            _persistenceImplementation.GetById(id).Returns(entity);

            _repository.GetById(id)
                .Result
                .Should()
                .BeNull();
        }

        [Test]
        public virtual void Delete_InvokesPersistenceLayer()
        {
            var entityToDelete = CreateTestEntity();
            
            _persistenceImplementation
                .Delete(entityToDelete)
                .Returns(Task.CompletedTask);
            
            _persistenceImplementation.ClearReceivedCalls();
            
            _repository
                .Delete(entityToDelete)
                .Wait();

            _persistenceImplementation
                .Received(1)
                .Delete(entityToDelete);
        }

        [Test]
        public virtual void DeleteById_InvokesPersistenceLayer()
        {
            var idToDelete = CreateId();
            
            _persistenceImplementation
                .DeleteById(idToDelete)
                .Returns(Task.CompletedTask);
            
            _persistenceImplementation.ClearReceivedCalls();
            
            _repository
                .DeleteById(idToDelete)
                .Wait();

            _persistenceImplementation
                .Received(1)
                .DeleteById(idToDelete);
        }
        
        [Test]
        public virtual void Insert_InvokesPersistenceLayerAndSetsCreatedAtAndLastModifiedAt()
        {
            var entityToInsert = CreateTestEntity();
            
            _persistenceImplementation
                .Insert(entityToInsert)
                .Returns(Task.CompletedTask);
            
            _persistenceImplementation.ClearReceivedCalls();
            
            _repository
                .Insert(entityToInsert)
                .Wait();

            _persistenceImplementation
                .Received(1)
                .Insert(entityToInsert);

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
        public virtual void Update_InvokesPersistenceLayerAndSetsLastModifiedAt()
        {
            var entityToUpdate = CreateTestEntity();
            
            _persistenceImplementation
                .Update(entityToUpdate)
                .Returns(Task.CompletedTask);
            
            _persistenceImplementation.ClearReceivedCalls();
            
            _repository
                .Update(entityToUpdate)
                .Wait();

            _persistenceImplementation
                .Received(1)
                .Update(entityToUpdate);

            entityToUpdate
                .LastModifiedAt
                .Should()
                .NotBe(default);
        }

        
        protected abstract TEntity CreateTestEntity();
        protected abstract TId CreateId();
    }
}