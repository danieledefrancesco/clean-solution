using AspNetCore.Examples.ProductService.Validators;
using FluentValidation;

namespace AspNetCore.Examples.ProductService.GetProductWithPriceCardById
{
    public sealed class GetProductWithPriceCardByIdRequestDtoValidator : AbstractValidator<GetProductWithPriceCardByIdRequestDto>
    {
        public GetProductWithPriceCardByIdRequestDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .ValidateProductId();
        }
    }
}