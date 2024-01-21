using System.Linq.Expressions;
using Business.Entities;
using Infrastructure.Data.DbContext;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Application.Validators;


public interface IHandlerValidator
{
    
    Task<bool> ActiveExpenseNotExistAsync(int id, Expression<Func<Expense, bool>> predicate, CancellationToken cancellationToken);
    Task<T> RecordExistAsync<T>(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) where T : class;
    
    Task<bool> RecordNotExistAsync<T>(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) where T : class;
    
}


public class HandlerValidator : IHandlerValidator
{
    private readonly ExpenseDbContext dbContext;

    public HandlerValidator(ExpenseDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    
    public async Task<bool> ActiveExpenseNotExistAsync(int id, Expression<Func<Expense, bool>> predicate, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Expenses.Where(predicate).FirstOrDefaultAsync(cancellationToken);

        if (entity != null)
        {
            throw new HttpException($"Active Expenses exist on this id: {id}", 405);
        }

        return true;
    }
    
    public async Task<T> RecordExistAsync<T>(Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken) where T : class
    {
        T? entity = await dbContext.Set<T>().FirstOrDefaultAsync<T>(predicate, cancellationToken);

        if (entity == null)
        {
            throw new HttpException($"No record found in {typeof(T).Name}", 404);
        }

        return entity;
    }
    
    public async Task<bool> RecordNotExistAsync<T>(Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken) where T : class
    {
        T? entity = await dbContext.Set<T>().FirstOrDefaultAsync<T>(predicate, cancellationToken);
        
        if (entity != null)
        {
            throw new HttpException($"Existing record in {typeof(T).Name}", 409);
        }

        return true;
    }
    
    // public async Task<T?> ValidateExistsAsync<T>(Func<IQueryable<T>, IQueryable<T>> query, CancellationToken cancellationToken) where T : class
    // {
    //     // T? entity = await query(dbContext.Set<T>()).FirstOrDefaultAsync(cancellationToken);
    //     T? entity = await query(dbContext.Set<T>()).FirstOrDefaultAsync(cancellationToken);
    //
    //     if (entity == null)
    //     {
    //         throw new HttpException($"{typeof(T)} record not found", 404);
    //     }
    //
    //     return entity;
    // }

    // public async Task<bool> ValidateNotExistsAsync<T>(Func<IQueryable<T>, IQueryable<T>> query, CancellationToken cancellationToken) where T : class
    // {
    //     T? entity = await query(dbContext.Set<T>()).FirstOrDefaultAsync(cancellationToken);
    //
    //     if (entity != null)
    //     {
    //         throw new HttpException($"Existing record in {typeof(T)}", 409);
    //     }
    //
    //     return true;
    // }


    // private string GetParameterName<T>(Expression<Func<T, bool>> predicate)
    // {
    //     if (predicate.Body is BinaryExpression binaryExpression)
    //     {
    //         if (binaryExpression.Left is MemberExpression left)
    //         {
    //             return left.Member.Name;
    //             
    //         }
    //     }

        // throw new ArgumentException("Unable to extract parameter name from the expression.", nameof(predicate));
    // }
    
    
    
}

