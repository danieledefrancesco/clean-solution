using AspNetCore.Examples.ProductService.Errors;
using Microsoft.AspNetCore.Http;

namespace AspNetCore.Examples.ProductService.ErrorHandlers
{
    public interface IErrorHandler
    {
        bool Supports(IError error);
        IResult HandleError(IError error);
    }
}