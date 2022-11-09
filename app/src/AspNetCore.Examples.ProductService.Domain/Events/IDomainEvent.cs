using System;
using MediatR;

namespace AspNetCore.Examples.ProductService.Events
{
    public interface IDomainEvent: INotification
    {
        public Guid Id { get; }
    }
}