using System;

namespace AspNetCore.Examples.ProductService.RemoteEventDefinitions
{
    public interface IRemoteEventDefinition
    {
        public Type DomainEventType { get; }
        public Type EventDtoType { get; }
    }
}