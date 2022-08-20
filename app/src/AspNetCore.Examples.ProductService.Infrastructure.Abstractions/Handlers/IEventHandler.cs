using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Events;

namespace AspNetCore.Examples.ProductService.Handlers
{
    public interface IEventHandler
    {
        Task RaiseEvent<T>(T @event) where T : EventBase;
    }
}