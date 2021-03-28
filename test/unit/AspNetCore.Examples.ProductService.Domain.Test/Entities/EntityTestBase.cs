using AspNetCore.Examples.ProductService.Common;
using FluentAssertions;
using NUnit.Framework;

namespace AspNetCore.Examples.ProductService.Entities
{
    public abstract class EntityTestBase<TEntity, TId>
        where TEntity : EntityBase<TId>
        where TId : class
    {

        [Test]
        public void GetHashCode_ReturnsIdGetHashCode()
        {
            var id = CreateId();
            var entity = CreateEntityById(id);

            entity
                .GetHashCode()
                .Should()
                .Be(id.GetHashCode());
        }

        [Test]
        public void Equals_ReturnsTrue_IfObjectsHaveSameReference()
        {
            var entity = CreateEntityById(CreateId());
            var entity2 = entity;
            entity
                .Equals(entity2)
                .Should()
                .BeTrue();
        }
        
        [Test]
        public void Equals_ReturnsFalse_IfOtherObjectIsNull()
        {
            var entity = CreateEntityById(CreateId());
            entity
                .Equals(null)
                .Should()
                .BeFalse();
        }
        
        [Test]
        public void Equals_ReturnsFalse_IfOtherObjectIsOfDifferentType()
        {
            var entity = CreateEntityById(CreateId());
            entity
                .Equals(new object())
                .Should()
                .BeFalse();
        }
        
        [Test]
        public void Equals_ReturnsTrue_IfOtherEntityHasSameId()
        {
            var id = CreateId();
            var entity = CreateEntityById(id);
            var otherEntity = CreateEntityById(id);
            entity
                .Equals(otherEntity)
                .Should()
                .BeTrue();
        }
        
        [Test]
        public void Equals_ReturnsFalse_IfOtherEntityHasDifferentId()
        {
            var entity = CreateEntityById(CreateId());
            var otherEntity = CreateEntityById(CreateDifferentId());
            entity
                .Equals(new object())
                .Should()
                .BeFalse();
        }
        
        protected abstract TId CreateId();
        protected abstract TId CreateDifferentId();

        protected abstract TEntity CreateEntityById(TId id);
    }
}