using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Attributes;
using AspNetCore.Examples.ProductService.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace AspNetCore.Examples.ProductService.Behaviors
{
    public class TransactionBehavior<TRequest,TResponse>: IPipelineBehavior<TRequest,TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IOneOf
    {
        private readonly IRequestHandler<TRequest, TResponse> _requestHandler;
        private readonly DbContext _dbContext;

        public TransactionBehavior(IRequestHandler<TRequest, TResponse> requestHandler, DbContext dbContext)
        {
            _requestHandler = requestHandler;
            _dbContext = dbContext;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (!RequestHandlerHasTransactionAttribute())
            {
                return await next();
            }
            try
            {
                return await PerformTransaction(cancellationToken, next);
            }
            catch (Exception)
            {
                await _dbContext.Database.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }

        private async Task<TResponse> PerformTransaction(CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            var result = await next();
            if (result.Value is IError)
            {
                await _dbContext.Database.RollbackTransactionAsync(cancellationToken);
                return result;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            await _dbContext.Database.CommitTransactionAsync(cancellationToken);
            return result;
        }

        private bool RequestHandlerHasTransactionAttribute()
        {
            return _requestHandler.GetType().GetCustomAttributes().Any(x => x is TransactionAttribute);
        }
    }
}