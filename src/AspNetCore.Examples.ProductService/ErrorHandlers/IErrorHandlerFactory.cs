using AspNetCore.Examples.ProductService.Errors;

namespace AspNetCore.Examples.ProductService.ErrorHandlers
{
    public interface IErrorHandlerFactory
    {
        IErrorHandler GetSupportingHandler(IError error);
    }
}