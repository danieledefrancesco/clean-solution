using AspNetCore.Examples.ProductService.DataTransferObjects;
using FluentValidation;

namespace AspNetCore.Examples.ProductService.Validators
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