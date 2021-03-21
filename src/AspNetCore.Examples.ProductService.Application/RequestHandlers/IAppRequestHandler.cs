using AspNetCore.Examples.ProductService.Errors;
using AspNetCore.Examples.ProductService.Requests;
using MediatR;
using OneOf;

namespace AspNetCore.Examples.ProductService.RequestHandlers
{
    public interface IAppRequestHandler<in TRequest, TResponse> : IRequestHandler<TRequest,OneOf<TResponse,IError>> 
        where TRequest : IAppRequest<TResponse>
    {

    }
}