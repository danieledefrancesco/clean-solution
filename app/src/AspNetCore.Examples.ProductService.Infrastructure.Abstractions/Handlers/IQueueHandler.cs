using System.Threading;
using System.Threading.Tasks;

namespace AspNetCore.Examples.ProductService.Handlers
{
    public interface IQueueHandler<in T>
    {
        Task SendMessageAsync(T @event, CancellationToken token);
    }
}