using AspNetCore.Examples.ProductService.Profiles;
using AutoMapper;

namespace AspNetCore.Examples.ProductService.GetProductWithPriceCardById
{
    public sealed class GetProductWithPriceCardByIdResponseProfileTest : ProfileTestBase<GetProductWithPriceCardByIdResponseProfile>
    {
        protected override void AddAdditionalConfig(IMapperConfigurationExpression mapperConfigurationExpression)
        {
            mapperConfigurationExpression.AddProfile<PriceCardProfile>();
        }
    }
}