using AspNetCore.Examples.ProductService.Products;
using AutoMapper;

namespace AspNetCore.Examples.ProductService.GetProductById
{
    public sealed class GetProductByIdRequestProfile: Profile
    {
        public GetProductByIdRequestProfile()
        {
            CreateMap<GetProductByIdRequestDto, GetProductByIdRequest>()
                .ConstructUsing(dto => new GetProductByIdRequest(ProductId.From(dto.ProductId)));
        }
    }
}