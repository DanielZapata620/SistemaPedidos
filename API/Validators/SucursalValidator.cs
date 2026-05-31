using API.Models.DTOs;
using FluentValidation;

namespace API.Validators
{
    public class CrearSucursalValidator : AbstractValidator<CrearSucursalDTO>
    {
        public CrearSucursalValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(120);
            RuleFor(x => x.Address).NotEmpty().MaximumLength(250);
            RuleFor(x => x.Username).NotEmpty().MaximumLength(80);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        }
    }

    public class EditarSucursalValidator : AbstractValidator<EditarSucursalDTO>
    {
        public EditarSucursalValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(120);
            RuleFor(x => x.Address).NotEmpty().MaximumLength(250);
            RuleFor(x => x.Username).NotEmpty().MaximumLength(80);
        }
    }
}
