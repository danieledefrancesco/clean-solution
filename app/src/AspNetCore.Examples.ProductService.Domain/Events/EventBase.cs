using System;

namespace AspNetCore.Examples.ProductService.Events
{
    public abstract class EventBase
    {
        public Guid Id { get; set; }
    }
}