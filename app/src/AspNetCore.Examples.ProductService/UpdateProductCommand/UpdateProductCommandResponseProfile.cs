using AutoMapper;

namespace AspNetCore.Examples.ProductService.UpdateProductCommand
{
    public sealed class UpdateProductCommandResponseProfile: Profile, ITypeConverter<UpdateProductCommandResponse, ProductDto>
    {
        public UpdateProductCommandResponseProfile()
        {
            CreateMap<UpdateProductCommandResponse, ProductDto>()
                .ConvertUsing(this);
        }

        public ProductDto Convert(UpdateProductCommandResponse source, ProductDto destination, ResolutionContext context) =>
            context.Mapper.Map<ProductDto>(source.UpdatedProduct);
    }
}