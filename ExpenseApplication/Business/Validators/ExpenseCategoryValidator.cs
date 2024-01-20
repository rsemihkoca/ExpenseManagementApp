using FluentValidation;
using Infrastructure.Dtos;

namespace Application.Validators;

public class CreateExpenseCategoryValidator : AbstractValidator<CreateExpenseCategoryRequest>
{
    public CreateExpenseCategoryValidator()
    {
        RuleFor(x => x.CategoryName)
            .NotEmpty()
            .WithMessage("Categoryname is required")
            .MaximumLength(255)
            .WithMessage("Categoryname must not exceed 255 characters");
    }
}

public class UpdateExpenseCategoryValidator : AbstractValidator<UpdateExpenseCategoryRequest>
{
    public UpdateExpenseCategoryValidator()
    {
        RuleFor(x => x.CategoryName)
            .NotEmpty()
            .WithMessage("Categoryname is required")
            .MaximumLength(255)
            .WithMessage("Categoryname must not exceed 255 characters");
    }
}