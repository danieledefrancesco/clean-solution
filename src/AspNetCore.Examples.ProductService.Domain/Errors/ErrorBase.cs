namespace AspNetCore.Examples.ProductService.Errors
{
    public abstract class ErrorBase : IError
    {
        public string Message { get; init; }
    }
}