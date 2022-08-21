using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OneOf;

namespace AspNetCore.Examples.ProductService.Behaviors
{
    public class TestHandler: IRequestHandler<TestRequest, IOneOf>
    {
        public Task<IOneOf> Handle(TestRequest request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}