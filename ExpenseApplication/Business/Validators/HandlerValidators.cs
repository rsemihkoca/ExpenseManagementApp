using System.Linq.Expressions;
using Business.Entities;
using Infrastructure.Data.DbContext;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Application.Validators;


public interface IHandlerValidator
{
    Task<User> ValidateUserIsExistAsync(int userId, CancellationToken cancellationToken);
    Task<ExpenseCategory> ValidateCategoryIsExistAsync(int categoryId, CancellationToken cancellationToken);

    Task<ExpenseCategory?> ValidateCategoryNameNotExistAsync(string modelCategoryName,
        CancellationToken cancellationToken);
    Task<bool> ValidateNoActiveEntityExistAsync(int id, Expression<Func<Expense, bool>> predicate, CancellationToken cancellationToken);
}


public class HandlerValidator : IHandlerValidator
{
    private readonly ExpenseDbContext dbContext;

    public HandlerValidator(ExpenseDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<User> ValidateUserIsExistAsync(int userId, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (user == null)
        {
            throw new HttpException("User not found", 404);
        }

        return user;
    }

    public async Task<ExpenseCategory> ValidateCategoryIsExistAsync(int categoryId, CancellationToken cancellationToken)
    {
        var category = await dbContext.ExpenseCategories.FirstOrDefaultAsync(x => x.CategoryId == categoryId, cancellationToken);

        if (category == null)
        {
            throw new HttpException("Category not found", 404);
        }
        
        return category;
    }
    
    public async Task<ExpenseCategory?> ValidateCategoryNameNotExistAsync(string categoryName, CancellationToken cancellationToken)
    {
        var category = await dbContext.ExpenseCategories.FirstOrDefaultAsync(x => x.CategoryName == categoryName.ToUpper(), cancellationToken);

        if (category != null)
        {
            throw new HttpException("Category name already exist", 409);
        }
        
        return category;
    }
    
    public async Task<bool> ValidateNoActiveEntityExistAsync(int id, Expression<Func<Expense, bool>> predicate, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Expenses.FirstOrDefaultAsync(predicate, cancellationToken);

        if (entity != null)
        {
            throw new HttpException($"Active Expenses exist on this id: {id}", 405);
        }

        return true;
    }
    
    
    
    
}

