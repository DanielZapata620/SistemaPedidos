using FluentValidation;
using PedidoApi.Models.Dtos;

namespace PedidoApi.Validators;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .Must((dto, value) => dto.Role == "admin" || value.Contains('@'))
            .WithMessage("Ingresa un correo valido o el usuario de sucursal.");
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        RuleFor(x => x.Role).Must(role => role is "cliente" or "admin");
    }
}

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(80);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        RuleFor(x => x.Role).Must(role => role is "cliente" or "admin");
        RuleFor(x => x.AuthProvider).Must(provider => provider is "local" or "google");
    }
}
