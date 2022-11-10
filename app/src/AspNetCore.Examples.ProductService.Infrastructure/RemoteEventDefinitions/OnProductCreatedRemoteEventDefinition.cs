using System;
using AspNetCore.Examples.ProductService.Products;

namespace AspNetCore.Examples.ProductService.RemoteEventDefinitions
{
    public sealed class OnProductCreatedRemoteEventDefinition: IRemoteEventDefinition
    {
        public Type DomainEventType => typeof(OnProductCreated);
        public Type EventDtoType => typeof(OnProductCreatedEventDto);
    }
}