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
    public sealed class TransactionBehavior<TRequest,TResponse>: IPipelineBehavior<TRequest,TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IOneOf
    {
        private readonly IRequestHandler<TRequest, TResponse> _requestHandler;
        private readonly DbContext _dbContext;
        private readonly IMediator _mediator;

        public TransactionBehavior(IRequestHandler<TRequest, TResponse> requestHandler, DbContext dbContext, IMediator mediator)
        {
            _requestHandler = requestHandler;
            _dbContext = dbContext;
            _mediator = mediator;
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

            await RaiseDomainEvents(cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await _dbContext.Database.CommitTransactionAsync(cancellationToken);
            return result;
        }

        private async Task RaiseDomainEvents(CancellationToken cancellationToken)
        {
            var changedEntities = 
                _dbContext.ChangeTracker?.Entries<IAggregateRoot>()?.ToList() ?? Enumerable.Empty<EntityEntry<IAggregateRoot>>();
            var tasks = changedEntities
                .SelectMany(e => RaiseAggregateEvents(e.Entity, cancellationToken));
            await Task.WhenAll(tasks);
            
        }

        private IEnumerable<Task> RaiseAggregateEvents(IAggregateRoot aggregateRoot, CancellationToken cancellationToken)
        {
            var events = aggregateRoot.DomainEvents;
            aggregateRoot.ClearDomainEvents();
            return events.Select(domainEvent => _mediator.Publish(domainEvent, cancellationToken));
        }


        private bool RequestHandlerHasTransactionAttribute()
        {
            return _requestHandler.GetType().GetCustomAttributes().Any(x => x is TransactionAttribute);
        }
    }
}