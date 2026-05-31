using FluentValidation;
using PedidoApi.Models.Dtos;

namespace PedidoApi.Validators;

public class ProductCreateDtoValidator : AbstractValidator<ProductCreateDto>
{
    public ProductCreateDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(80);
        RuleFor(x => x.Description).MaximumLength(250);
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.ImageUrl).NotEmpty().MaximumLength(300);
    }
}

public class ProductUpdateDtoValidator : AbstractValidator<ProductUpdateDto>
{
    public ProductUpdateDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(80);
        RuleFor(x => x.Description).MaximumLength(250);
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.ImageUrl).NotEmpty().MaximumLength(300);
    }
}
