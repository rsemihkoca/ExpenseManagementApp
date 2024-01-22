using FluentValidation;
using Schemes.Dtos;

namespace Business.Validators;

public class CreateExpenseValidator : AbstractValidator<CreateExpenseRequest>
{
    
    public CreateExpenseValidator()
    {
    
        RuleFor(expense => expense.UserId)
            .NotEmpty().WithMessage(Constants.ExpenseValidationMessages.UserIdRequired)
            .GreaterThan(0).WithMessage(Constants.ExpenseValidationMessages.UserIdGreaterThanZero);
        
        RuleFor(expense => expense.Amount)
            .NotEmpty().WithMessage(Constants.ExpenseValidationMessages.AmountRequired)
            .GreaterThan(0).WithMessage(Constants.ExpenseValidationMessages.AmountGreaterThanZero);

        // must be integer
        RuleFor(expense => expense.CategoryId)
            .NotEmpty().WithMessage(Constants.ExpenseValidationMessages.CategoryIdRequired)
            .GreaterThan(0).WithMessage(Constants.ExpenseValidationMessages.CategoryIdGreaterThanZero);
        
        RuleFor(expense => expense.PaymentMethod)
            .NotEmpty().WithMessage(Constants.ExpenseValidationMessages.PaymentMethodRequired)
            .MaximumLength(50).WithMessage(Constants.ExpenseValidationMessages.PaymentMethodMaxLength);

        RuleFor(expense => expense.PaymentLocation)
            .NotEmpty().WithMessage(Constants.ExpenseValidationMessages.PaymentLocationRequired)
            .MaximumLength(255).WithMessage(Constants.ExpenseValidationMessages.PaymentLocationMaxLength);

        RuleFor(expense => expense.Documents)
            .NotEmpty().WithMessage(Constants.ExpenseValidationMessages.DocumentsPathRequired)
            .MaximumLength(255).WithMessage(Constants.ExpenseValidationMessages.DocumentsPathMaxLength);
        
        RuleFor(expense => expense.Description)
            .NotEmpty().WithMessage(Constants.ExpenseValidationMessages.DescriptionRequired)
            .MaximumLength(255).WithMessage(Constants.ExpenseValidationMessages.DescriptionMaxLength);
    }
}


public class UpdateExpenseValidator : AbstractValidator<UpdateExpenseRequest>
{
    public UpdateExpenseValidator()
    {
        RuleFor(expense => expense.UserId)
            .NotEmpty().WithMessage(Constants.ExpenseValidationMessages.UserIdRequired)
            .GreaterThan(0).WithMessage(Constants.ExpenseValidationMessages.UserIdGreaterThanZero);
        
        RuleFor(expense => expense.Amount)
            .NotEmpty().WithMessage(Constants.ExpenseValidationMessages.AmountRequired)
            .GreaterThan(0).WithMessage(Constants.ExpenseValidationMessages.AmountGreaterThanZero);

        RuleFor(expense => expense.CategoryId)
            .NotEmpty().WithMessage(Constants.ExpenseValidationMessages.CategoryIdRequired)
            .GreaterThan(0).WithMessage(Constants.ExpenseValidationMessages.CategoryIdGreaterThanZero);
        
        RuleFor(expense => expense.PaymentMethod)
            .NotEmpty().WithMessage(Constants.ExpenseValidationMessages.PaymentMethodRequired)
            .MaximumLength(50).WithMessage(Constants.ExpenseValidationMessages.PaymentMethodMaxLength);

        RuleFor(expense => expense.PaymentLocation)
            .NotEmpty().WithMessage(Constants.ExpenseValidationMessages.PaymentLocationRequired)
            .MaximumLength(255).WithMessage(Constants.ExpenseValidationMessages.PaymentLocationMaxLength);

        RuleFor(expense => expense.Documents)
            .NotEmpty().WithMessage(Constants.ExpenseValidationMessages.DocumentsPathRequired)
            .MaximumLength(255).WithMessage(Constants.ExpenseValidationMessages.DocumentsPathMaxLength);
            
        RuleFor(expense => expense.Status)
            .NotEmpty().WithMessage(Constants.ExpenseValidationMessages.StatusRequired)
            .Must(x => x.ToString().Equals(Constants.ExpenseRequestStatus.Pending) || x.ToString().Equals(Constants.ExpenseRequestStatus.Approved) || x.ToString().Equals(Constants.ExpenseRequestStatus.Rejected))
            .WithMessage(Constants.ExpenseValidationMessages.StatusInvalid);
        
        RuleFor(expense => expense.Description)
            .NotEmpty().WithMessage(Constants.ExpenseValidationMessages.DescriptionRequired)
            .MaximumLength(255).WithMessage(Constants.ExpenseValidationMessages.DescriptionMaxLength);
            
        RuleFor(expression => expression.PaymentStatus)
            .NotEmpty().WithMessage(Constants.ExpenseValidationMessages.PaymentStatusRequired)
            .Must(x => x.ToString().Equals(Constants.PaymentRequestStatus.Pending) || x.ToString().Equals(Constants.PaymentRequestStatus.Declined) || x.ToString().Equals(Constants.PaymentRequestStatus.Completed) || x.ToString().Equals(Constants.PaymentRequestStatus.Failed))
            .WithMessage(Constants.ExpenseValidationMessages.PaymentStatusInvalid);

        RuleFor(expense => expense.PaymentDescription)
            .NotEmpty().WithMessage(Constants.ExpenseValidationMessages.PaymentDescriptionRequired)
            .MaximumLength(255).WithMessage(Constants.ExpenseValidationMessages.PaymentDescriptionMaxLength);
    }
}


public class GetExpenseByParameterRequestValidator : AbstractValidator<GetExpenseByParameterRequest>
{
    public GetExpenseByParameterRequestValidator()
    {
        RuleFor(request => request.UserId)
            .GreaterThan(0).When(request => request.UserId.HasValue)
            .WithMessage(Constants.ExpenseValidationMessages.UserIdGreaterThanZeroWhenProvided);

        RuleFor(request => request.CategoryId)
            .GreaterThan(0).When(request => request.CategoryId.HasValue)
            .WithMessage(Constants.ExpenseValidationMessages.CategoryIdGreaterThanZeroWhenProvided);

        RuleFor(request => request.Status)
            .Must(BeAValidExpenseStatus)
            .When(request => !string.IsNullOrEmpty(request.Status))
            .WithMessage(Constants.ExpenseValidationMessages.StatusInvalid);

        RuleFor(request => request.PaymentStatus)
            .Must(BeAValidPaymentStatus)
            .When(request => !string.IsNullOrEmpty(request.PaymentStatus))
            .WithMessage(Constants.ExpenseValidationMessages.PaymentStatusInvalid);
    }

    private bool BeAValidExpenseStatus(string status)
    {
        return string.IsNullOrEmpty(status) || 
               status.Equals(Constants.ExpenseRequestStatus.Pending, StringComparison.OrdinalIgnoreCase) || 
               status.Equals(Constants.ExpenseRequestStatus.Approved, StringComparison.OrdinalIgnoreCase) || 
               status.Equals(Constants.ExpenseRequestStatus.Rejected, StringComparison.OrdinalIgnoreCase);
    }

    private bool BeAValidPaymentStatus(string paymentStatus)
    {
        return string.IsNullOrEmpty(paymentStatus) || 
               paymentStatus.Equals(Constants.PaymentRequestStatus.Pending, StringComparison.OrdinalIgnoreCase) || 
               paymentStatus.Equals(Constants.PaymentRequestStatus.Declined, StringComparison.OrdinalIgnoreCase) ||
               paymentStatus.Equals(Constants.PaymentRequestStatus.Completed, StringComparison.OrdinalIgnoreCase) || 
               paymentStatus.Equals(Constants.PaymentRequestStatus.Failed, StringComparison.OrdinalIgnoreCase);
    }
    
}