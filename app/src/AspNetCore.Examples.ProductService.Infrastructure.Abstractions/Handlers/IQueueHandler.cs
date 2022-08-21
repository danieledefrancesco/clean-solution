using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Events;

namespace AspNetCore.Examples.ProductService.Handlers
{
    public interface IQueueHandler<in T> where T : EventBase
    {
        Task SendMessageAsync(T @event, CancellationToken token);
    }
}