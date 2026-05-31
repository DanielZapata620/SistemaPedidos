using FluentValidation;
using PedidoApi.Models.Dtos;

namespace PedidoApi.Validators;

public class OrderCreateDtoValidator : AbstractValidator<OrderCreateDto>
{
    public OrderCreateDtoValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
        RuleFor(x => x.BranchId).GreaterThan(0);
        RuleFor(x => x.Items).NotEmpty();
        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(x => x.ProductId).GreaterThan(0);
            item.RuleFor(x => x.Quantity).InclusiveBetween(1, 50);
        });
    }
}
