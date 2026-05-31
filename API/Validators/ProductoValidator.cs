using API.Models.DTOs;
using FluentValidation;

namespace API.Validators
{
    public class CrearProductoValidator : AbstractValidator<CrearProductoDTO>
    {
        public CrearProductoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(120);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(400);
            RuleFor(x => x.Price).GreaterThan(0);
        }
    }

    public class EditarProductoValidator : AbstractValidator<EditarProductoDTO>
    {
        public EditarProductoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(120);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(400);
            RuleFor(x => x.Price).GreaterThan(0);
        }
    }
}
