using System.Collections.Generic;
using System.Linq;
using AspNetCore.Examples.ProductService.Errors;

namespace AspNetCore.Examples.ProductService.ErrorHandlers
{
    public sealed class ErrorHandlerFactory : IErrorHandlerFactory
    {
        private readonly IEnumerable<IErrorHandler> _errorHandlers;

        public ErrorHandlerFactory(IEnumerable<IErrorHandler> errorHandlers)
        {
            _errorHandlers = errorHandlers;
        }

        public IErrorHandler GetSupportingHandler(IError error)
        {
            return _errorHandlers.FirstOrDefault(handler => handler.Supports(error));
        }
    }
}