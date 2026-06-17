using Application.DTOs.Usuarios;
using FluentValidation;

namespace Application.Validators;

public class CreateUsuarioDtoValidator : AbstractValidator<CreateUsuarioDto>
{
    public CreateUsuarioDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(255);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        RuleFor(x => x.Name).MaximumLength(255);
        RuleFor(x => x.Phone).MaximumLength(50);
    }
}

public class UpdateUsuarioDtoValidator : AbstractValidator<UpdateUsuarioDto>
{
    public UpdateUsuarioDtoValidator()
    {
        RuleFor(x => x.Name).MaximumLength(255);
        RuleFor(x => x.Phone).MaximumLength(50);
    }
}
