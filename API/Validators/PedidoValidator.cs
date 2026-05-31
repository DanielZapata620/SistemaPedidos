using API.Models.DTOs;
using FluentValidation;

namespace API.Validators
{
    public class CrearPedidoValidator : AbstractValidator<CrearPedidoDTO>
    {
        public CrearPedidoValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);
            RuleFor(x => x.BranchId).GreaterThan(0);
            RuleFor(x => x.Items).NotEmpty();
            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(x => x.ProductId).GreaterThan(0);
                item.RuleFor(x => x.Quantity).GreaterThan(0);
            });
        }
    }

    public class EstadoPedidoValidator : AbstractValidator<EstadoPedidoDTO>
    {
        public EstadoPedidoValidator()
        {
            RuleFor(x => x.Status)
                .NotEmpty()
                .Must(x => x == "enviado" || x == "en preparacion" || x == "listo para recoger");
        }
    }
}
