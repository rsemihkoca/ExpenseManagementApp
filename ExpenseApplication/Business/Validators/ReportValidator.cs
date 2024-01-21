using FluentValidation;
using Infrastructure.Dtos;

namespace Application.Validators;


public class ReportValidator : AbstractValidator<ReportFrequencyRequest>
{
    public ReportValidator()
    {
        RuleFor(x => x.Type)
        .NotEmpty().WithMessage("Type is required")
        .Must(x => x == "daily" || x == "weekly" || x == "monthly")
        .WithMessage("Type must be daily, weekly or monthly");
    }
}