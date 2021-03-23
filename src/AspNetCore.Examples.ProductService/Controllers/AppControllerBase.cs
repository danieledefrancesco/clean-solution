using System;
using System.Threading.Tasks;
using AspNetCore.Examples.ProductService.ErrorHandlers;
using AspNetCore.Examples.ProductService.Errors;
using AspNetCore.Examples.ProductService.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Examples.ProductService.Controllers
{
    public abstract class AppControllerBase: ControllerBase
    {
        private readonly IErrorHandlerFactory _errorHandlerFactory;
        private readonly IMediator _mediator;

        protected AppControllerBase(IErrorHandlerFactory errorHandlerFactory, IMediator mediator)
        {
            _errorHandlerFactory = errorHandlerFactory;
            _mediator = mediator;
        }

        private IActionResult Error(IError error)
        {
            return _errorHandlerFactory
                .GetSupportingHandler(error)
                .HandleError(error);
        }
        
        public virtual async Task<IActionResult> MediatorResponse<TRequest,TResponse>(
            TRequest request,
            Func<TResponse, object> map)
        where TRequest : IAppRequest<TResponse>
        {
            var response = await _mediator.Send(request);
            return response.Match(
                result => Ok(map(result)),
                Error
            );
        }
    }
}