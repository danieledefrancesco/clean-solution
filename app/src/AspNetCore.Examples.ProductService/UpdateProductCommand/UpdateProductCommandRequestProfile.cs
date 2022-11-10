using AspNetCore.Examples.ProductService.Products;
using AutoMapper;

namespace AspNetCore.Examples.ProductService.UpdateProductCommand
{
    public sealed class UpdateProductCommandRequestProfile: Profile
    {
        public UpdateProductCommandRequestProfile()
        {
            CreateMap<UpdateProductCommandRequestDto, UpdateProductCommandRequest>()
                .ConstructUsing(dto => new UpdateProductCommandRequest(
                    ProductId.From(dto.Id),
                    ProductName.From(dto.Body.Name),
                    ProductPrice.From(dto.Body.Price)));
        }
    }
}