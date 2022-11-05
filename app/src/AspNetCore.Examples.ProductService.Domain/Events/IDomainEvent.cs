using MediatR;

namespace AspNetCore.Examples.ProductService.Events
{
    public interface IDomainEvent: INotification
    {
        
    }
}