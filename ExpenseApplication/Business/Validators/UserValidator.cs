using FluentValidation;
using IbanNet;
using IbanNet.FluentValidation;
using Infrastructure.Dtos;

namespace Application.Validators;

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator(IIbanValidator ibanValidator)
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters.")
            .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.");

        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
            .MaximumLength(255).WithMessage("Password cannot exceed 255 characters.")
            .Matches(@"^(?=.*[!@#$%^&*(),.?\\\"":{}|<>])(?=.*[0-9])(?=.*[A-Z])(?=.*[a-z]).*$")
            .WithMessage("Password must contain at least 1 special character, 1 number, 1 uppercase letter, and 1 lowercase letter.");

        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is required.")
            .MinimumLength(2).WithMessage("First Name must be at least 2 characters.")
            .MaximumLength(50).WithMessage("First Name cannot exceed 50 characters.");

        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name is required.")
            .MinimumLength(2).WithMessage("Last Name must be at least 2 characters.")
            .MaximumLength(50).WithMessage("Last Name cannot exceed 50 characters.");

        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.")
            .MaximumLength(255).WithMessage("Email cannot exceed 255 characters.");

        RuleFor(x => x.Iban).NotEmpty().WithMessage("IBAN is required.")
            .Iban(ibanValidator).WithMessage("Invalid IBAN.")
            .MaximumLength(34).WithMessage("IBAN cannot exceed 34 characters.");

        RuleFor(x => x.Role).NotEmpty().WithMessage("Role is required.")
            .Must(role => role == "Admin" || role == "Personnel").WithMessage("Invalid role.")
            .MaximumLength(30).WithMessage("Role cannot exceed 30 characters.");

    }
}

public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserValidator(IIbanValidator ibanValidator)
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters.")
            .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.");

        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Your password length must be at least 8.")
            .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
            .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
            .Matches(@"[\!\?\*\.]+").WithMessage("Your password must contain at least one (!? *.).");

        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is required.")
            .MinimumLength(2).WithMessage("First Name must be at least 2 characters.")
            .MaximumLength(50).WithMessage("First Name cannot exceed 50 characters.");

        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name is required.")
            .MinimumLength(2).WithMessage("Last Name must be at least 2 characters.")
            .MaximumLength(50).WithMessage("Last Name cannot exceed 50 characters.");

        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.")
            .MaximumLength(255).WithMessage("Email cannot exceed 255 characters.");

        RuleFor(x => x.Iban).NotEmpty().WithMessage("IBAN is required.")
            .Iban(ibanValidator).WithMessage("Invalid IBAN.")
            .MaximumLength(34).WithMessage("IBAN cannot exceed 34 characters.");

        RuleFor(x => x.Role).NotEmpty().WithMessage("Role is required.")
            .Must(role => role == "Admin" || role == "Personnel").WithMessage("Invalid role.")
            .MaximumLength(30).WithMessage("Role cannot exceed 30 characters.");

        // RuleFor(x => x.IsActive) //.NotEmpty().WithMessage("IsActive is required.");

    }
}
