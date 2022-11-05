using MediatR;
using OneOf;

namespace AspNetCore.Examples.ProductService.Behaviors
{
    public sealed class TestRequest: IRequest<IOneOf>
    {
        
    }
}