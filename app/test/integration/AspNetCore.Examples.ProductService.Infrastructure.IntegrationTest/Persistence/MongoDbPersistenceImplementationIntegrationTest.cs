using System;
using AspNetCore.Examples.ProductService.Common;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.Persistence
{
    public class MongoDbPersistenceImplementationIntegrationTest : InfrastructureIntegrationTestBase
    {

        private MongoDbPersistenceImplementation<TestEntity, string> _mongoDbPersistenceImplementation;
        private IMongoCollection<TestEntity> _collection;

        [SetUp]
        public void SetUp()
        {
            _mongoDbPersistenceImplementation = ActivatorUtilities
                .GetServiceOrCreateInstance<MongoDbPersistenceImplementation<TestEntity, string>>(ServiceProvider);
            
            _collection = _mongoDbPersistenceImplementation.RootCollection;
            ResetCollection();
        }

        private void ResetCollection()
        {
            _collection.DeleteMany(x => true);
        }

        [Test]
        public void Dummy()
        {
            _collection.InsertOne(new TestEntity()
            {
                Id = "a"
            });
            _collection.Find(x => x.Id == "a").FirstOrDefault().Should().NotBeNull();
        }

        [Test]
        public void GetById_ReturnsEntity_IfEntityExists()
        {
            var expectedEntity = new TestEntity()
            {
                Id = "id1"
            };
            _collection.InsertOne(expectedEntity);

            var actualEntity = _mongoDbPersistenceImplementation
                .GetById(expectedEntity.Id)
                .Result;

            actualEntity
                .Should()
                .Be(expectedEntity);
        }
        
        [Test]
        public void GetById_ReturnsNull_IfEntityDoesntExists()
        {
            var actualEntity = _mongoDbPersistenceImplementation
                .GetById("id1")
                .Result;

            actualEntity
                .Should()
                .BeNull();
        }

        [Test]
        public void Insert_InsertsNewEntity_IfEntityDoesntExist()
        {
            var expectedEntity = new TestEntity()
            {
                Id = "id1"
            };

            _mongoDbPersistenceImplementation.Insert(expectedEntity).Wait();

            var actualEntity = _collection
                .Find(x => x.Id == expectedEntity.Id)
                .SingleOrDefault();

            actualEntity
                .Should()
                .Be(expectedEntity);
        }

        [Test]
        public void Insert_ThrowsException_IfEntityAlreadyExists()
        {
            var entity = new TestEntity()
            {
                Id = "id1"
            };
            
            _collection.InsertOne(entity);

            Action insertEntityAction = _mongoDbPersistenceImplementation
                .Insert(entity)
                .Wait;

            insertEntityAction
                .Should()
                .Throw<Exception>();

        }

        [Test]
        public void Update_UpdatesEntity_IfEntityExists()
        {
            var entity = new TestEntity()
            {
                Id = "id1"
            };
            
            _collection.InsertOne(entity);

            var expectedDate = new DateTime(2021, 1, 1);
            entity.LastModifiedAt = expectedDate;

            _mongoDbPersistenceImplementation
                .Update(entity)
                .Wait();

            var updatedEntity = _collection
                .Find(x => x.Id == entity.Id)
                .SingleOrDefault();

            updatedEntity.LastModifiedAt
                .Should()
                .Be(expectedDate);
        }
        
        [Test]
        public void Update_DoesNothing_IfEntityDoesntExist()
        {
            var entity = new TestEntity()
            {
                Id = "id1"
            };
            
            var expectedDate = new DateTime(2021, 1, 1);
            entity.LastModifiedAt = expectedDate;

            _mongoDbPersistenceImplementation
                .Update(entity)
                .Wait();

            var updatedEntity = _collection
                .Find(x => x.Id == entity.Id)
                .SingleOrDefault();

            updatedEntity
                .Should()
                .BeNull();
        }

        [Test]
        public void Delete_DeletesEntity_IfEntityExists()
        {
            var entity = new TestEntity()
            {
                Id = "id1"
            };
            
            _collection.InsertOne(entity);
            
            _mongoDbPersistenceImplementation
                .Delete(entity)
                .Wait();

            var deletedEntity = _collection
                .Find(x => x.Id == entity.Id)
                .SingleOrDefault();

            deletedEntity
                .Should()
                .BeNull();
        }
        
        [Test]
        public void Delete_DoesNothing_IfEntityDoesntExist()
        {
            var entity = new TestEntity()
            {
                Id = "id1"
            };
            
            _mongoDbPersistenceImplementation
                .Delete(entity)
                .Wait();

            var deletedEntity = _collection
                .Find(x => x.Id == entity.Id)
                .SingleOrDefault();

            deletedEntity
                .Should()
                .BeNull();
        }
        
        [Test]
        public void DeleteById_DeletesEntity_IfEntityExists()
        {
            var entity = new TestEntity()
            {
                Id = "id1"
            };
            
            _collection.InsertOne(entity);
            
            _mongoDbPersistenceImplementation
                .DeleteById(entity.Id)
                .Wait();

            var deletedEntity = _collection
                .Find(x => x.Id == entity.Id)
                .SingleOrDefault();

            deletedEntity
                .Should()
                .BeNull();
        }
        
        [Test]
        public void DeleteById_DoesNothing_IfEntityDoesntExist()
        {
            var entity = new TestEntity()
            {
                Id = "id1"
            };
            
            _mongoDbPersistenceImplementation
                .DeleteById(entity.Id)
                .Wait();

            var deletedEntity = _collection
                .Find(x => x.Id == entity.Id)
                .SingleOrDefault();

            deletedEntity
                .Should()
                .BeNull();
        }

    }
}