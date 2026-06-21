using Application.DTOs.Talonarios;
using FluentValidation;

namespace Application.Validators;

public class CreateTalonarioDtoValidator : AbstractValidator<CreateTalonarioDto>
{
    public CreateTalonarioDtoValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(255);
        RuleFor(x => x.BoletasNumber).GreaterThan(0);
        RuleFor(x => x.BoletasValue).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Jackpot).NotEmpty().MaximumLength(255);
    }
}

public class UpdateTalonarioDtoValidator : AbstractValidator<UpdateTalonarioDto>
{
    public UpdateTalonarioDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(255);
        RuleFor(x => x.BoletasNumber).GreaterThan(0);
        RuleFor(x => x.BoletasValue).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Jackpot).NotEmpty().MaximumLength(255);
    }
}
