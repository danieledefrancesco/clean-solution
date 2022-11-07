using AspNetCore.Examples.ProductService.Responses;
using AutoMapper;

namespace AspNetCore.Examples.ProductService.Profiles
{
    public sealed class GetProductByIdResponseProfile: Profile, ITypeConverter<GetProductByIdResponse, ProductDto>
    {
        public GetProductByIdResponseProfile()
        {
            CreateMap<GetProductByIdResponse, ProductDto>()
                .ConvertUsing(this);
        }

        public ProductDto Convert(GetProductByIdResponse source, ProductDto destination, ResolutionContext context) =>
            context.Mapper.Map<ProductDto>(source.Product);
    }
}