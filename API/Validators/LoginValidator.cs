using API.Models.DTOs;
using FluentValidation;

namespace API.Validators
{
    public class LoginValidator : AbstractValidator<LoginDTO>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Escribe tu correo o usuario.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Escribe tu contrasena.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Selecciona cliente o negocio.");
        }
    }
}
