using AspNetCore.Examples.ProductService.Errors;
using MediatR;
using OneOf;

namespace AspNetCore.Examples.ProductService.Requests
{
    public interface IAppRequest<T> : IRequest<OneOf<T,ErrorBase>>

    {

    }
}