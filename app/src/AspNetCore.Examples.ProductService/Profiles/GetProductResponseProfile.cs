using AspNetCore.Examples.ProductService.Entities;
using AspNetCore.Examples.ProductService.Responses;
using AutoMapper;

namespace AspNetCore.Examples.ProductService.Profiles
{
    public class GetProductResponseProfile: Profile, ITypeConverter<GetProductByIdResponse, ProductDto>
    {
        public GetProductResponseProfile()
        {
            CreateMap<GetProductByIdResponse, ProductDto>()
                .ConvertUsing(this);
        }
        public ProductDto Convert(GetProductByIdResponse source, ProductDto destination, ResolutionContext context) =>
            context.Mapper.Map<ProductDto>(source.Product);
    }
}