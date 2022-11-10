using System;

namespace AspNetCore.Examples.ProductService.Events
{
    public abstract class DomainEventBase : IDomainEvent
    {
        protected DomainEventBase(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; }
    }
}