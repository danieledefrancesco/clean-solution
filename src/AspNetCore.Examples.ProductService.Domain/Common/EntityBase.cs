using System;
using System.Collections.Generic;

namespace AspNetCore.Examples.ProductService.Common
{
    public abstract class EntityBase<T>
        where T : class
    {
        public T Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            var other = (EntityBase<T>) obj;
            return this.Id?.Equals(other.Id) ?? false;
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(Id);
        }
    }
}