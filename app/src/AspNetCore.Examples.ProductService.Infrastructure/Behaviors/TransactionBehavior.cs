using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Attributes;
using AspNetCore.Examples.ProductService.Entities;
using AspNetCore.Examples.ProductService.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OneOf;

namespace AspNetCore.Examples.ProductService.Behaviors
{
    public sealed class TransactionBehavior<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> requestHandler,
            DbContext dbContext, IMediator mediator)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IOneOf
    {
        private async Task<TResponse> PerformTransaction(CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            await dbContext.Database.BeginTransactionAsync(cancellationToken);
            var result = await next();
            if (result.Value is IError)
            {
                await dbContext.Database.RollbackTransactionAsync(cancellationToken);
                return result;
            }

            await RaiseDomainEvents(cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            await dbContext.Database.CommitTransactionAsync(cancellationToken);
            return result;
        }

        private async Task RaiseDomainEvents(CancellationToken cancellationToken)
        {
            var changedEntities = 
                dbContext.ChangeTracker?.Entries<IAggregateRoot>()?.ToList() ?? Enumerable.Empty<EntityEntry<IAggregateRoot>>();
            var tasks = changedEntities
                .SelectMany(e => RaiseAggregateEvents(e.Entity, cancellationToken));
            await Task.WhenAll(tasks);
            
        }

        private IEnumerable<Task> RaiseAggregateEvents(IAggregateRoot aggregateRoot, CancellationToken cancellationToken)
        {
            var events = aggregateRoot.DomainEvents;
            aggregateRoot.ClearDomainEvents();
            return events.Select(domainEvent => mediator.Publish(domainEvent, cancellationToken));
        }


        private bool RequestHandlerHasTransactionAttribute()
        {
            return requestHandler.GetType().GetCustomAttributes().Any(x => x is TransactionAttribute);
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
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
                await dbContext.Database.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
}