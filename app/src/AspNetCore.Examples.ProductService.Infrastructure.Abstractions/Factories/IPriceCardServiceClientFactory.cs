using AspNetCore.Examples.PriceCardService;

namespace AspNetCore.Examples.ProductService.Factories
{
    public interface IPriceCardServiceClientFactory
    {
        PriceCardServiceClient Create();
    }
}