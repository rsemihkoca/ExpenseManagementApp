using System.Linq.Expressions;
using Business.Services;
using Infrastructure.DbContext;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Schemes.Enums;
using Schemes.Exceptions;

namespace Business.Validators;


public interface IHandlerValidator
{
    
    // Task<bool> ActiveExpenseNotExistAsync(int id, Expression<Func<Expense, bool>> predicate, CancellationToken cancellationToken);
    Task<T> RecordExistAsync<T>(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) where T : class;
    
    Task<bool> RecordNotExistAsync<T>(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) where T : class;

    Task<(string role, int creatorId)> UserAuthAsync(int? modelUserId, CancellationToken cancellationToken);
    
    Task<bool> ExpenseCanBeApprovedAsync(Expense fromdb, CancellationToken cancellationToken);
    
    Task<bool> ExpenseCanBeRejectedAsync(Expense fromdb, CancellationToken cancellationToken);
    
    Task<bool> IdGreaterThanZeroAsync(int id, CancellationToken cancellationToken);
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
            case Constants.Roles.Admin:
                break;
            case Constants.Roles.Personnel:
                if (userId != creatorId) // null or any other user id
                {
                    throw new HttpException(Constants.ErrorMessages.NotOwnId, 403);
                }
                
                break;
            default:
                throw new HttpException(Constants.ErrorMessages.UnauthorizedRole, 403);
        }
        
        return (role, creatorId);
    }

    // ExpenseCanBeApprovedAsync
    public async Task<bool> ExpenseCanBeApprovedAsync(Expense fromdb, CancellationToken cancellationToken)
    {
        
        if (fromdb.Status == ExpenseRequestStatus.Approved && fromdb.PaymentStatus == PaymentRequestStatus.Completed)
            throw new HttpException(Constants.ErrorMessages.MadePayment, 405);
        if (fromdb.Status == ExpenseRequestStatus.Approved && fromdb.PaymentStatus == PaymentRequestStatus.OnProcess)
            throw new HttpException(Constants.ErrorMessages.InProgressPayment, 405);
        else
        {
            return true;

        }
    }
    public async Task<bool> ExpenseCanBeRejectedAsync(Expense fromdb, CancellationToken cancellationToken)
    {
        
        if (fromdb.PaymentStatus == PaymentRequestStatus.Completed)
            throw new HttpException(Constants.ErrorMessages.MadePayment, 405);
        if (fromdb.Status == ExpenseRequestStatus.Approved && fromdb.PaymentStatus == PaymentRequestStatus.OnProcess)
            throw new HttpException(Constants.ErrorMessages.InProgressPayment, 405);
        else
        {
            return true;

        }
    }
    
    /* Check Id is greater than zero */
    public async Task<bool> IdGreaterThanZeroAsync(int id, CancellationToken cancellationToken)    
    {
        if (id <= 0)
        {
            throw new HttpException(Constants.ErrorMessages.IdLessThanZero, 400);
        }

        return true;
    }
}

