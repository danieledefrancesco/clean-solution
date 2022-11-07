using AutoMapper;

namespace AspNetCore.Examples.ProductService.Profiles
{
    public sealed class GetProductWithPriceCardByIdResponseProfileTest : ProfileTestBase<GetProductWithPriceCardByIdResponseProfile>
    {
        protected override void AddAdditionalConfig(IMapperConfigurationExpression mapperConfigurationExpression)
        {
            mapperConfigurationExpression.AddProfile<PriceCardProfile>();
        }
    }
}