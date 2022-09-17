using AspNetCore.Examples.ProductService.ErrorHandlers;
using AspNetCore.Examples.ProductService.Requests;
using AutoMapper;
using MediatR;

namespace AspNetCore.Examples.ProductService.Endpoints
{
    public abstract class EndpointTestBase<TEndpoint, TRequest, TAppRequest, TAppResponse, TResponse>
        where TEndpoint : EndpointBase<TRequest, TAppRequest, TAppResponse, TResponse>
        where TAppRequest : IAppRequest<TAppResponse>
    {
        private IMapper _mapper;
        private IMediator _mediator;
        private IErrorHandlerFactory _errorHandlerFactory;
    }
}