using FluentValidation;
using PedidoApi.Models.Dtos;

namespace PedidoApi.Validators;

public class BranchCreateDtoValidator : AbstractValidator<BranchCreateDto>
{
    public BranchCreateDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Address).NotEmpty().MaximumLength(250);
        RuleFor(x => x.Username).NotEmpty().MinimumLength(3).MaximumLength(80);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}

public class BranchUpdateDtoValidator : AbstractValidator<BranchUpdateDto>
{
    public BranchUpdateDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Address).NotEmpty().MaximumLength(250);
        RuleFor(x => x.Username).NotEmpty().MinimumLength(3).MaximumLength(80);
        RuleFor(x => x.Password).MinimumLength(6).When(x => !string.IsNullOrWhiteSpace(x.Password));
    }
}
