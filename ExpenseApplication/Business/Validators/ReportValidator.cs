using FluentValidation;
using Infrastructure.Dtos;

namespace Business.Validators;


public class ReportValidator : AbstractValidator<ReportFrequencyRequest>
{
    public ReportValidator()
    {
        RuleFor(x => x.Type)
        .NotEmpty().WithMessage(Constants.ReportValidationMessages.TypeRequired)
        .Must(x => x == Constants.Frequency.Daily || x == Constants.Frequency.Weekly || x == Constants.Frequency.Monthly)
        .WithMessage(Constants.ReportValidationMessages.InvalidType);
    }
}

public class PersonnelSummaryValidator : AbstractValidator<PersonnelSummaryRequest>
{
    public PersonnelSummaryValidator()
    {
        RuleFor(x => x.Type)
            .NotEmpty().WithMessage(Constants.PersonnelSummaryValidationMessages.TypeRequired)
            .Must(x => x == Constants.Frequency.Daily || x == Constants.Frequency.Weekly || x == Constants.Frequency.Monthly)
            .WithMessage(Constants.PersonnelSummaryValidationMessages.InvalidType);
        
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage(Constants.PersonnelSummaryValidationMessages.UserIdGreaterThanZero);
    }
}