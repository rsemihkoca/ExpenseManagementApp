using FluentValidation;
using Infrastructure.Dtos;

namespace Application.Validators;

public class ExpenseCategoryValidator : AbstractValidator<CreateExpenseCategoryRequest>
{
    public ExpenseCategoryValidator()
    {
        RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Name is required");
    }
}

public class UpdateExpenseCategoryValidator : AbstractValidator<UpdateExpenseCategoryRequest>
{
    public UpdateExpenseCategoryValidator()
    {
        RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Name is required");
    }
}