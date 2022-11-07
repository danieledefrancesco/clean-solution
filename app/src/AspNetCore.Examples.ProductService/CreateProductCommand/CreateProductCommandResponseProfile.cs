using AutoMapper;

namespace AspNetCore.Examples.ProductService.CreateProductCommand
{
    public sealed class CreateProductCommandResponseProfile : Profile, ITypeConverter<CreateProductCommandResponse, ProductDto>
    {
        public CreateProductCommandResponseProfile()
        {
            CreateMap<CreateProductCommandResponse, ProductDto>()
                .ConvertUsing(this);
        }

        public ProductDto Convert(CreateProductCommandResponse source, ProductDto destination,
            ResolutionContext context) => context.Mapper.Map<ProductDto>(source.CreatedProduct);
    }
}