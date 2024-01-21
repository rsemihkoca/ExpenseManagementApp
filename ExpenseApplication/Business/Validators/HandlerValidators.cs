using System.Linq.Expressions;
using Application.Services;
using Business.Entities;
using Business.Enums;
using Infrastructure.Data.DbContext;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Application.Validators;


public interface IHandlerValidator
{
    
    // Task<bool> ActiveExpenseNotExistAsync(int id, Expression<Func<Expense, bool>> predicate, CancellationToken cancellationToken);
    Task<T> RecordExistAsync<T>(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) where T : class;
    
    Task<bool> RecordNotExistAsync<T>(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) where T : class;

    Task<(string role, int creatorId)> UserAuthAsync(int? modelUserId, CancellationToken cancellationToken);
    
    Task<bool> ExpenseCanBeApprovedAsync(Expense fromdb, CancellationToken cancellationToken);
}


public class HandlerValidator : IHandlerValidator
{
    private readonly ExpenseDbContext dbContext;
    private readonly IUserService userService;

    public HandlerValidator(ExpenseDbContext dbContext, IUserService userService)
    {
        this.dbContext = dbContext;
        this.userService = userService;
    }
    
    // public async Task<bool> ActiveExpenseNotExistAsync(int id, Expression<Func<Expense, bool>> predicate, CancellationToken cancellationToken)
    // {
    //     var entity = await dbContext.Expenses.Where(predicate).FirstOrDefaultAsync(cancellationToken);
    //
    //     if (entity != null)
    //     {
    //         throw new HttpException($"Active Expenses exist on this id: {id}", 405);
    //     }
    //
    //     return true;
    // }
    
    public async Task<T> RecordExistAsync<T>(Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken) where T : class
    {
        T? entity = await dbContext.Set<T>().FirstOrDefaultAsync<T>(predicate, cancellationToken);

        if (entity == null)
        {
            throw new HttpException($"No record found in {predicate.Parameters[0].Type.FullName}", 404);
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
    
    public async Task<(string, int)> UserAuthAsync(int? userId, CancellationToken cancellationToken)
    {
        var role = userService.GetUserRole();
        var creatorId = userService.GetUserId();
        switch (role)
        {
            case "Admin":
                break;
            case "Personnel":
                if (userId != creatorId) // null or any other user id
                {
                    throw new HttpException($"Please enter your own id", 403);
                }
                
                break;
            default:
                throw new HttpException("Unauthorized role", 403);
        }
        
        return (role, creatorId);
    }

    // ExpenseCanBeApprovedAsync
    public async Task<bool> ExpenseCanBeApprovedAsync(Expense fromdb, CancellationToken cancellationToken)
    {
        
        if (fromdb.Status != ExpenseRequestStatus.Approved && fromdb.PaymentStatus != PaymentRequestStatus.Completed)
            return true;
        else
        {
            throw new HttpException("Expense already approved and payment completed", 405);
        }
    }
    
    
}

