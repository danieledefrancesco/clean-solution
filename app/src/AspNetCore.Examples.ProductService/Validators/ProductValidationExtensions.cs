using FluentValidation;

namespace AspNetCore.Examples.ProductService.Validators
{
    public static class ProductValidationExtensions
    {
        public static void ValidateProductId<T>(this IRuleBuilderInitial<T, string> ruleBuilder)
        {
            ruleBuilder
                .NotNull()
                .NotEmpty()
                .Matches("^[a-zA-Z]([a-zA-Z0-9-_ ]*)$");

        }
    }
}