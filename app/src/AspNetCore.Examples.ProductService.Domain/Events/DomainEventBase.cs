using System;

namespace AspNetCore.Examples.ProductService.Events
{
    public abstract class DomainEventBase : IDomainEvent
    {
        public Guid Id { get; set; }
    }
}