using System.Collections.Generic;
using System.Collections.Immutable;
using AspNetCore.Examples.ProductService.Events;

namespace AspNetCore.Examples.ProductService.Entities
{
    public abstract class AggregateRootBase<T>: EntityBase<T>, IAggregateRoot<T> where T : class
    {
        protected AggregateRootBase(T id) : base(id)
        {
        }
        private readonly List<IDomainEvent> _domainEvents = new ();
        public IEnumerable<IDomainEvent> DomainEvents => _domainEvents.ToImmutableList();
        public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}