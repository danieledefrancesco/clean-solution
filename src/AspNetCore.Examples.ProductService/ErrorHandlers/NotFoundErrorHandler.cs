using AspNetCore.Examples.ProductService.Errors;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Examples.ProductService.ErrorHandlers
{
    public class NotFoundErrorHandler : IErrorHandler
    {
        public bool Supports(IError error)
        {
            return error is NotFoundError;
        }

        public IActionResult HandleError(IError error)
        {
            return new NotFoundObjectResult(error);
        }
    }
}