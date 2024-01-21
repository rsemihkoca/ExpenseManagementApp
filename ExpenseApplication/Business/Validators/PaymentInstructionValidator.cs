using FluentValidation;
using Infrastructure.Dtos;

namespace Application.Validators;

public class CreatePaymentInstructionValidator : AbstractValidator<CreatePaymentInstructionRequest>
{
    public CreatePaymentInstructionValidator()
    {
        RuleFor(x => x.ExpenseRequestId)
            .NotEmpty().WithMessage("ExpenseRequestId is required.")
            .GreaterThan(0).WithMessage("ExpenseRequestId must be greater than zero.");
        
        
        RuleFor(x => x.PaymentDate)
            .NotEmpty().WithMessage("PaymentDate is required.")
            .Must(x => x != null && x.Value.Date <= DateTime.Now.Date)
            .WithMessage("PaymentDate must be less than or equal to today's date.");
            
        RuleFor(x => x.PaymentDescription)
            .NotEmpty().WithMessage("PaymentDescription is required.")
            .MaximumLength(255).WithMessage("PaymentDescription cannot exceed 255 characters.");
    }
}

public class UpdatePaymentInstructionValidator : AbstractValidator<UpdatePaymentInstructionRequest>
{
    public UpdatePaymentInstructionValidator()
    {
        RuleFor(x => x.PaymentStatus)
            .NotEmpty().WithMessage("PaymentStatus is required.")
            .Must(x => x.ToString().Equals("Pending") || x.ToString().Equals("Completed") || x.ToString().Equals("Failed"))
            .WithMessage("PaymentStatus must be one of the following: Pending, Completed, Failed.");
        
        RuleFor(x => x.PaymentDate)
            .NotEmpty().WithMessage("PaymentDate is required.")
            .Must(x => x != null && x.Value.Date <= DateTime.Now.Date)
            .WithMessage("PaymentDate must be less than or equal to today's date.");
            
        RuleFor(x => x.PaymentDescription)
            .NotEmpty().WithMessage("PaymentDescription is required.")
            .MaximumLength(255).WithMessage("PaymentDescription cannot exceed 255 characters.");
        

    }
}