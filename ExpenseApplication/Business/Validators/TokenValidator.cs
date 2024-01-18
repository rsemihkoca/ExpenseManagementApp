using FluentValidation;
using Infrastructure.Dtos;

namespace Application.Validators;

public class CreateTokenValidator : AbstractValidator<TokenRequest>
{
    public CreateTokenValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Password).NotEmpty().MaximumLength(255);
    }
}