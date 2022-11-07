using AspNetCore.Examples.ProductService.DataTransferObjects;
using AspNetCore.Examples.ProductService.Products;
using AutoMapper;

namespace AspNetCore.Examples.ProductService.GetProductWithPriceCardById
{
    public sealed class GetProductWithPriceCardByIdRequestProfile: Profile
    {
        public GetProductWithPriceCardByIdRequestProfile()
        {
            CreateMap<GetProductWithPriceCardByIdRequestDto, GetProductWithPriceCardByIdRequest>()
                .ConstructUsing(dto => new GetProductWithPriceCardByIdRequest(ProductId.From(dto.ProductId)));
        }
    }
}