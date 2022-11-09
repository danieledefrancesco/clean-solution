using AspNetCore.Examples.ProductService.Validators;
using FluentValidation;

namespace AspNetCore.Examples.ProductService.UpdateProductCommand
{
    public sealed class UpdateProductCommandRequestDtoValidator: AbstractValidator<UpdateProductCommandRequestDto>
    {
        public UpdateProductCommandRequestDtoValidator()
        {
            RuleFor(x => x).NotNull();
            RuleFor(x => x.Id)
                .ValidateProductId();
            RuleFor(x => x.Body).NotNull();
            RuleFor(x => x.Body.Name)
                .NotNullOrWhiteSpace();
            RuleFor(x => x.Body.Price).NotNull();
        }
    }
}