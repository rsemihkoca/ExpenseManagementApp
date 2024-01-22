using FluentValidation;
using Infrastructure.Dtos;
using Schemes.Dtos;

namespace Business.Validators;

public class CreateTokenValidator : AbstractValidator<TokenRequest>
{
    public CreateTokenValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Password).NotEmpty().MaximumLength(255);
    }
}