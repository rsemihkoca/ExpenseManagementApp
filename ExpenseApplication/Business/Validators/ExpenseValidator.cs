using FluentValidation;
using Infrastructure.Dtos;
using Microsoft.AspNetCore.Components;

namespace Application.Validators;

public class CreateExpenseValidator : AbstractValidator<CreateExpenseRequest>
{
    
    public CreateExpenseValidator()
    {
    
        RuleFor(expense => expense.UserId)
            .NotEmpty().WithMessage("User id is required.")
            .GreaterThan(0).WithMessage("User id must be greater than zero.");
        
        RuleFor(expense => expense.Amount)
            .NotEmpty().WithMessage("Amount is required.")
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");

        // must be integer
        RuleFor(expense => expense.CategoryId)
            .NotEmpty().WithMessage("Category is required.")
            .GreaterThan(0).WithMessage("Category must be greater than zero.");
        
        RuleFor(expense => expense.PaymentMethod)
            .NotEmpty().WithMessage("Payment method is required.")
            .MaximumLength(50).WithMessage("Payment method cannot exceed 50 characters.");

        RuleFor(expense => expense.PaymentLocation)
            .NotEmpty().WithMessage("Payment location is required.")
            .MaximumLength(255).WithMessage("Payment location cannot exceed 255 characters.");

        RuleFor(expense => expense.Documents)
            .NotEmpty().WithMessage("Documents path is required.")
            .MaximumLength(255).WithMessage("Documents path cannot exceed 255 characters.");
        
        RuleFor(expense => expense.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(255).WithMessage("Description cannot exceed 255 characters.");
    }
}


public class UpdateExpenseValidator : AbstractValidator<UpdateExpenseRequest>
{
    public UpdateExpenseValidator()
    {
        RuleFor(expense => expense.UserId)
            .NotEmpty().WithMessage("User id is required.")
            .GreaterThan(0).WithMessage("User id must be greater than zero.");
        
        RuleFor(expense => expense.Amount)
            .NotEmpty().WithMessage("Amount is required.")
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");

        // must be integer
        RuleFor(expense => expense.CategoryId)
            .NotEmpty().WithMessage("Category is required.")
            .GreaterThan(0).WithMessage("Category must be greater than zero.");
        
        RuleFor(expense => expense.PaymentMethod)
            .NotEmpty().WithMessage("Payment method is required.")
            .MaximumLength(50).WithMessage("Payment method cannot exceed 50 characters.");

        RuleFor(expense => expense.PaymentLocation)
            .NotEmpty().WithMessage("Payment location is required.")
            .MaximumLength(255).WithMessage("Payment location cannot exceed 255 characters.");

        RuleFor(expense => expense.Documents)
            .NotEmpty().WithMessage("Documents path is required.")
            .MaximumLength(255).WithMessage("Documents path cannot exceed 255 characters.");
            
        //     public ExpenseRequestStatus Status { get; set; }
        // must be valid enum
        RuleFor(expense => expense.Status)
            .NotEmpty().WithMessage("Status is required.")
            .IsInEnum().WithMessage("Status must be valid.");
        
        RuleFor(expense => expense.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(255).WithMessage("Description cannot exceed 255 characters.");
    }
}


public class GetExpenseByParameterRequestValidator : AbstractValidator<GetExpenseByParameterRequest>
{
    public GetExpenseByParameterRequestValidator()
    {
        RuleFor(request => request.UserId)
            .GreaterThan(0).When(request => request.UserId.HasValue)
            .WithMessage("UserId must be greater than 0 when provided.");

        RuleFor(request => request.CategoryId)
            .GreaterThan(0).When(request => request.CategoryId.HasValue)
            .WithMessage("CategoryId must be greater than 0 when provided.");

        RuleFor(request => request.Status)
            .Must(BeAValidExpenseStatus)
            .When(request => !string.IsNullOrEmpty(request.Status))
            .WithMessage("Invalid expense status. Accepted values are: Pending, Approved, Rejected");

        RuleFor(request => request.PaymentStatus)
            .Must(BeAValidPaymentStatus)
            .When(request => !string.IsNullOrEmpty(request.PaymentStatus))
            .WithMessage("Invalid payment status. Accepted values are: Pending, Completed, Failed");
    }

    private bool BeAValidExpenseStatus(string status)
    {
        return string.IsNullOrEmpty(status) || 
               status.Equals("Pending", StringComparison.OrdinalIgnoreCase) || 
               status.Equals("Approved", StringComparison.OrdinalIgnoreCase) || 
               status.Equals("Rejected", StringComparison.OrdinalIgnoreCase);
    }

    private bool BeAValidPaymentStatus(string paymentStatus)
    {
        return string.IsNullOrEmpty(paymentStatus) || 
               paymentStatus.Equals("Pending", StringComparison.OrdinalIgnoreCase) || 
               paymentStatus.Equals("Completed", StringComparison.OrdinalIgnoreCase) || 
               paymentStatus.Equals("Failed", StringComparison.OrdinalIgnoreCase);
    }
    
}