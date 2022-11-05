using System;
using System.Collections.Generic;
using AspNetCore.Examples.ProductService.Events;

namespace AspNetCore.Examples.ProductService.Entities
{
    public interface IAggregateRoot
    {
        public IEnumerable<IDomainEvent> DomainEvents { get; }
        public void AddDomainEvent(IDomainEvent domainDomainEvent);
        public void ClearDomainEvents();
    }

    public interface IAggregateRoot<out T> : IAggregateRoot, IEntity<T>
    {
        
    }
}