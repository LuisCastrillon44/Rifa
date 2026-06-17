using Application.DTOs.Boletas;
using FluentValidation;

namespace Application.Validators;

public class CreateBoletaDtoValidator : AbstractValidator<CreateBoletaDto>
{
    public CreateBoletaDtoValidator()
    {
        RuleFor(x => x.TalonarioId).GreaterThan(0);
        RuleFor(x => x.Number).GreaterThan(0);
        RuleFor(x => x.BuyerName).MaximumLength(255);
        RuleFor(x => x.BuyerPhone).MaximumLength(50);
    }
}

public class UpdateBoletaDtoValidator : AbstractValidator<UpdateBoletaDto>
{
    public UpdateBoletaDtoValidator()
    {
        RuleFor(x => x.BuyerName).MaximumLength(255);
        RuleFor(x => x.BuyerPhone).MaximumLength(50);
    }
}
