using AspNetCore.Examples.ProductService.Errors;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Examples.ProductService.ErrorHandlers
{
    public class DefaultErrorHandler : IErrorHandler
    {
        public bool Supports(IError error)
        {
            return true;
        }

        public IActionResult HandleError(IError error)
        {
            return new ObjectResult(error)
            {
                StatusCode = 400
            };
        }
    }
}