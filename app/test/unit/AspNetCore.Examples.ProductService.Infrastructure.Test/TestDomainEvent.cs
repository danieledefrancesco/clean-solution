using System;
using AspNetCore.Examples.ProductService.Events;

namespace AspNetCore.Examples.ProductService
{
    public sealed class TestDomainEvent: DomainEventBase
    {
        public TestDomainEvent(Guid id) : base(id)
        {
        }
    }
}