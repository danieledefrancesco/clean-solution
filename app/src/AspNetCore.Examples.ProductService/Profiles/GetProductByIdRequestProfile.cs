using AspNetCore.Examples.ProductService.DataTransferObjects;
using AspNetCore.Examples.ProductService.Requests;
using AspNetCore.Examples.ProductService.ValueObjects;
using AutoMapper;

namespace AspNetCore.Examples.ProductService.Profiles
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