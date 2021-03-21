namespace AspNetCore.Examples.ProductService.Errors
{
    public interface IError
    {
        string Message { get; init; }
    }
}