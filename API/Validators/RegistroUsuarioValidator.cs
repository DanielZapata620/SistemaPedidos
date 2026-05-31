using API.Models.DTOs;
using FluentValidation;

namespace API.Validators
{
    public class RegistroUsuarioValidator : AbstractValidator<RegistroUsuarioDTO>
    {
        public RegistroUsuarioValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Escribe tu nombre.")
                .MaximumLength(120).WithMessage("El nombre no puede tener mas de 120 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Escribe tu correo.")
                .EmailAddress().WithMessage("Escribe un correo valido.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Escribe tu contrasena.")
                .MinimumLength(6).WithMessage("La contrasena debe tener minimo 6 caracteres.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Selecciona cliente o negocio.")
                .Must(x => x == "cliente" || x == "admin").WithMessage("Tipo de cuenta no valido.");
        }
    }
}
