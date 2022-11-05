using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.Attributes;
using MediatR;
using OneOf;

namespace AspNetCore.Examples.ProductService.Behaviors
{
    [Transaction]
    public sealed class TestTransactionHandler: IRequestHandler<TestRequest, IOneOf>
    {
        public Task<IOneOf> Handle(TestRequest request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}