using AspNetCore.Examples.ProductService.Errors;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Examples.ProductService.ErrorHandlers
{
    public interface IErrorHandler
    {
        bool Supports(IError error);
        IActionResult HandleError(IError error);
    }
}