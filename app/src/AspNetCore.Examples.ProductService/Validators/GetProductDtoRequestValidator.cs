using AspNetCore.Examples.ProductService.Requests;
using FluentValidation;

namespace AspNetCore.Examples.ProductService.Validators
{
    public sealed class GetProductDtoRequestValidator : AbstractValidator<GetProductWithPriceCardByIdRequestDto>
    {
        public GetProductDtoRequestValidator()
        {
            RuleFor(x => x.ProductId)
                .ValidateProductId();
        }
    }
}