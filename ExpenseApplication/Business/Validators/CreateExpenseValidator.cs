using FluentValidation;
using Infrastructure.Dtos;

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

// public double Amount { get; set; }
// public int CategoryId { get; set; }
// public string PaymentMethod { get; set; }
// public string PaymentLocation { get; set; }
//
// public string Documents { get; set; }
//
// // public ExpenseRequestStatus Status { get; set; } // (Pending
// public string Description { get; set; }