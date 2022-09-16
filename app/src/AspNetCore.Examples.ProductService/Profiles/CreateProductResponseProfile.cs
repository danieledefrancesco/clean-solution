using AspNetCore.Examples.ProductService.Entities;
using AspNetCore.Examples.ProductService.Responses;
using AutoMapper;

namespace AspNetCore.Examples.ProductService.Profiles
{
    public class CreateProductResponseProfile: Profile, ITypeConverter<CreateProductCommandResponse, ProductDto>
    {
        public CreateProductResponseProfile()
        {
            CreateMap<CreateProductCommandResponse, ProductDto>()
                .ConvertUsing(this);
        }
        public ProductDto Convert(CreateProductCommandResponse source, ProductDto destination, ResolutionContext context) =>
            context.Mapper.Map<ProductDto>(source.CreatedProduct);
    }
}