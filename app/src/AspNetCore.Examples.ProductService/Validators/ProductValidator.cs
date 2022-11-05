using FluentValidation;

namespace AspNetCore.Examples.ProductService.Validators
{
    public sealed class ProductValidator : AbstractValidator<ProductDto>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Id)
                .ValidateProductId();
            RuleFor(x => x.Name)
                .NotNullOrWhiteSpace();
        }
    }
}