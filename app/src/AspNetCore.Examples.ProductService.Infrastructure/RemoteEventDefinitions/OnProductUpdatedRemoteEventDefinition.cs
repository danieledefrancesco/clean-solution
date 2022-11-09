using System;
using AspNetCore.Examples.ProductService.Products;

namespace AspNetCore.Examples.ProductService.RemoteEventDefinitions
{
    public sealed class OnProductUpdatedRemoteEventDefinition: IRemoteEventDefinition

    {
        public Type DomainEventType => typeof(OnProductUpdated);
        public Type EventDtoType => typeof(OnProductUpdatedEventDto);
    }
}