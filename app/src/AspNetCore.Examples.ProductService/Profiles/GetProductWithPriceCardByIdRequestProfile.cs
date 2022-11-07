using AspNetCore.Examples.ProductService.DataTransferObjects;
using AspNetCore.Examples.ProductService.Requests;
using AspNetCore.Examples.ProductService.ValueObjects;
using AutoMapper;

namespace AspNetCore.Examples.ProductService.Profiles
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