using FluentValidation;
using IbanNet;
using IbanNet.FluentValidation;
using Schemes.Dtos;

namespace Business.Validators;

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator(IIbanValidator ibanValidator)
    {

        RuleFor(x => x.Role).NotEmpty().WithMessage("Role is required.")
            .Must(role => role == Constants.Roles.Admin || role == Constants.Roles.Personnel).WithMessage("Invalid role.")
            .MaximumLength(30).WithMessage("Role cannot exceed 30 characters.");
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage(Constants.UserValidationMessages.Required("Username"))
            .Length(3, 50).WithMessage(Constants.UserValidationMessages.InvalidLength("Username", 3, 50));

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(Constants.UserValidationMessages.Required("Password"))
            .Length(8, 255).WithMessage(Constants.UserValidationMessages.InvalidLength("Password", 8, 255))
            .Matches(@"^(?=.*[!@#$%^&*(),.?\\\"":{}|<>])(?=.*[0-9])(?=.*[A-Z])(?=.*[a-z]).*$")
            .WithMessage(Constants.UserValidationMessages.InvalidPasswordMessage);

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(Constants.UserValidationMessages.Required("First Name"))
            .Length(2, 50).WithMessage(Constants.UserValidationMessages.InvalidLength("First Name", 2, 50));

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(Constants.UserValidationMessages.Required("Last Name"))
            .Length(2, 50).WithMessage(Constants.UserValidationMessages.InvalidLength("Last Name", 2, 50));

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(Constants.UserValidationMessages.Required("Email"))
            .EmailAddress().WithMessage(Constants.UserValidationMessages.InvalidEmailMessage)
            .MaximumLength(255).WithMessage(Constants.UserValidationMessages.InvalidLength("Email", 0, 255));

        RuleFor(x => x.Iban)
            .NotEmpty().WithMessage(Constants.UserValidationMessages.Required("IBAN"))
            .Iban(ibanValidator).WithMessage(Constants.UserValidationMessages.InvalidIbanMessage)
            .MaximumLength(34).WithMessage(Constants.UserValidationMessages.InvalidLength("IBAN", 0, 34));

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage(Constants.UserValidationMessages.Required("Role"))
            .Must(role => role == Constants.Roles.Admin || role == Constants.Roles.Personnel)
            .WithMessage(Constants.UserValidationMessages.InvalidRoleMessage)
            .MaximumLength(30).WithMessage(Constants.UserValidationMessages.InvalidLength("Role", 0, 30));
            
    }
}

public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserValidator(IIbanValidator ibanValidator)
    {
        RuleFor(x => x.Role).NotEmpty().WithMessage("Role is required.")
            .Must(role => role == Constants.Roles.Admin || role == Constants.Roles.Personnel).WithMessage("Invalid role.")
            .MaximumLength(30).WithMessage("Role cannot exceed 30 characters.");
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage(Constants.UserValidationMessages.Required("Username"))
            .Length(3, 50).WithMessage(Constants.UserValidationMessages.InvalidLength("Username", 3, 50));

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(Constants.UserValidationMessages.Required("Password"))
            .Length(8, 255).WithMessage(Constants.UserValidationMessages.InvalidLength("Password", 8, 255))
            .Matches(@"^(?=.*[!@#$%^&*(),.?\\\"":{}|<>])(?=.*[0-9])(?=.*[A-Z])(?=.*[a-z]).*$")
            .WithMessage(Constants.UserValidationMessages.InvalidPasswordMessage);

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(Constants.UserValidationMessages.Required("First Name"))
            .Length(2, 50).WithMessage(Constants.UserValidationMessages.InvalidLength("First Name", 2, 50));

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(Constants.UserValidationMessages.Required("Last Name"))
            .Length(2, 50).WithMessage(Constants.UserValidationMessages.InvalidLength("Last Name", 2, 50));

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(Constants.UserValidationMessages.Required("Email"))
            .EmailAddress().WithMessage(Constants.UserValidationMessages.InvalidEmailMessage)
            .MaximumLength(255).WithMessage(Constants.UserValidationMessages.InvalidLength("Email", 0, 255));

        RuleFor(x => x.Iban)
            .NotEmpty().WithMessage(Constants.UserValidationMessages.Required("IBAN"))
            .Iban(ibanValidator).WithMessage(Constants.UserValidationMessages.InvalidIbanMessage)
            .MaximumLength(34).WithMessage(Constants.UserValidationMessages.InvalidLength("IBAN", 0, 34));

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage(Constants.UserValidationMessages.Required("Role"))
            .Must(role => role == Constants.Roles.Admin || role == Constants.Roles.Personnel)
            .WithMessage(Constants.UserValidationMessages.InvalidRoleMessage)
            .MaximumLength(30).WithMessage(Constants.UserValidationMessages.InvalidLength("Role", 0, 30));
            
        RuleFor(x => x.IsActive).NotEmpty().WithMessage("IsActive is required.");


    }
}

