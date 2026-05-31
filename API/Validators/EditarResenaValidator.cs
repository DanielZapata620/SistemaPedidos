using API.Models.DTOs;
using FluentValidation;

namespace API.Validators
{
    public class EditarResenaValidator : AbstractValidator<EditarResenaDTO>
    {
        public EditarResenaValidator()
        {
            RuleFor(x => x.NombrePlatillo)
                .NotEmpty().WithMessage("El nombre del platillo es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre del platillo no puede tener mas de 100 caracteres.");

            RuleFor(x => x.Calificacion)
                .InclusiveBetween(1, 5).WithMessage("La calificacion debe estar entre 1 y 5.");

            RuleFor(x => x.UbicacionEstablecimiento)
                .NotEmpty().WithMessage("La ubicacion del establecimiento es obligatoria.")
                .MaximumLength(200).WithMessage("La ubicacion no puede tener mas de 200 caracteres.");

            RuleFor(x => x.Telefono).Matches(@"^\d{10}$").WithMessage("El telefono debe tener exactamente 10 numeros.")
                .When(x => !string.IsNullOrWhiteSpace(x.Telefono));
        }
    }
}
