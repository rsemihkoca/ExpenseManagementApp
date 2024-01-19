using Business.Entities;
using Infrastructure.Data.DbContext;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Application.Validators;


public interface IHandlerValidator
{
    Task<Boolean> ValidateUserAsync(int userId, CancellationToken cancellationToken);
    Task<Boolean> ValidateCategoryAsync(int categoryId, CancellationToken cancellationToken);
}



public class HandlerValidator : IHandlerValidator
{
    private readonly ExpenseDbContext dbContext;

    public HandlerValidator(ExpenseDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<bool> ValidateUserAsync(int userId, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (user == null)
        {
            throw new HttpException("User not found", 404);
        }

        return true;
    }

    public async Task<bool> ValidateCategoryAsync(int categoryId, CancellationToken cancellationToken)
    {
        var category = await dbContext.ExpenseCategories.FirstOrDefaultAsync(x => x.CategoryId == categoryId, cancellationToken);

        if (category == null)
        {
            throw new HttpException("Category not found", 404);
        }
        
        return true;
    }
}

