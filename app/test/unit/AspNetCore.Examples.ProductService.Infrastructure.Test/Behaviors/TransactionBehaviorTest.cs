using System;
using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Errors;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NSubstitute;
using NUnit.Framework;
using OneOf;

namespace AspNetCore.Examples.ProductService.Behaviors
{
    public sealed class TransactionBehaviorTest
    {
        private DbContext _dbContext;
        private DatabaseFacade _database;
        private IOneOf _oneOf;
        private RequestHandlerDelegate<IOneOf> _delegate;
        private IMediator _mediator;

        [SetUp]
        public void SetUp()
        {
            _dbContext = Substitute.For<DbContext>();
            _database = Substitute.For<DatabaseFacade>(_dbContext);
            _dbContext.Database.Returns(_database);
            _oneOf = Substitute.For<IOneOf>();
            _delegate = () => Task.FromResult(_oneOf);
            _mediator = Substitute.For<IMediator>();
        }
        private TransactionBehavior<TestRequest, IOneOf> CreateBehaviorWithTransactionRequestHandler()
        {
            return new TransactionBehavior<TestRequest, IOneOf>(new TestTransactionHandler(), _dbContext, _mediator);
        }

        private TransactionBehavior<TestRequest, IOneOf> CreateBehaviorWithRequestHandler()
        {
            return new TransactionBehavior<TestRequest, IOneOf>(new TestHandler(), _dbContext, _mediator);
        }

        [Test]
        public async Task Handle_DoesntBeginTransactionAndReturnsDelegateResponse_IfHandlerDoesntHaveTransactionAttribute()
        {
            var transactionBehavior = CreateBehaviorWithRequestHandler();
            var actualResult = await transactionBehavior.Handle(new TestRequest(), _delegate, CancellationToken.None);
            actualResult.Should().Be(_oneOf);
            await _database.Received(0).BeginTransactionAsync();
        }
        
        [Test]
        public async Task Handle_BeginsAndCommitsTransactionAndReturnsDelegateResponse_IfHandlerHaveTransactionAttributeAndDoesntReturnError()
        {
            var transactionBehavior = CreateBehaviorWithTransactionRequestHandler();
            var oneOfValue = new object();
            _oneOf.Value.Returns(oneOfValue);
            var actualResult = await transactionBehavior.Handle(new TestRequest(), _delegate, CancellationToken.None);
            actualResult.Should().Be(_oneOf);
            await _database.Received(1).BeginTransactionAsync();
            await _database.Received(1).CommitTransactionAsync();
        }

        [Test]
        public async Task Handle_RollsBackTransactionAndReturnsDelegateResponse_IfHandlerHaveTransactionAttributeAndReturnsError()
        {
            var transactionBehavior = CreateBehaviorWithTransactionRequestHandler();
            var oneOfValue = Substitute.For<IError>();
            _oneOf.Value.Returns(oneOfValue);
            var actualResult = await transactionBehavior.Handle(new TestRequest(), _delegate, CancellationToken.None);
            actualResult.Should().Be(_oneOf);
            await _database.Received(1).BeginTransactionAsync();
            await _database.Received(1).RollbackTransactionAsync();
        }
        
        [Test]
        public async Task Handle_RollsBackTransactionAndThrowsException_IfHandlerHaveTransactionAttributeAndThrowsException()
        {
            var transactionBehavior = CreateBehaviorWithTransactionRequestHandler();
            var oneOfValue = Substitute.For<IError>();
            _oneOf.Value.Returns(oneOfValue);
            Func<Task> act = () => transactionBehavior.Handle(new TestRequest(),
                () => throw new Exception(),
                CancellationToken.None);
            await act.Should().ThrowAsync<Exception>();
            await _database.Received(1).BeginTransactionAsync(); 
            await _database.Received(1).RollbackTransactionAsync();
        }
    }
}