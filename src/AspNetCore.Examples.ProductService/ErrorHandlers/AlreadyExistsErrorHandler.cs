using AspNetCore.Examples.ProductService.Errors;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Examples.ProductService.ErrorHandlers
{
    public class AlreadyExistsErrorHandler : IErrorHandler
    {
        public bool Supports(IError error)
        {
            return error is AlreadyExistsError;
        }

        public IActionResult HandleError(IError error)
        {
            return new ObjectResult(error)
            {
                StatusCode = 409
            };
        }
    }
}