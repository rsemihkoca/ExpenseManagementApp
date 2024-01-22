using Business.Cqrs;
using Business.Validators;
using AutoMapper;
using Infrastructure.DbContext;
using Infrastructure.Entities;
using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Schemes.Dtos;
using Schemes.Enums;

namespace Business.Queries;

public class ExpenseQueryHandler :
    IRequestHandler<GetAllExpenseQuery, List<ExpenseResponse>>,
    IRequestHandler<GetExpenseByIdQuery, ExpenseResponse>,
    IRequestHandler<GetExpenseByParameterQuery, List<ExpenseResponse>>

{
    private readonly ExpenseDbContext dbContext;
    private readonly IMapper mapper;
    private readonly IHandlerValidator validate;

    public ExpenseQueryHandler(ExpenseDbContext dbContext, IMapper mapper, IHandlerValidator validate)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
        this.validate = validate;
    }

    public async Task<List<ExpenseResponse>> Handle(GetAllExpenseQuery request, CancellationToken cancellationToken)
    {
        var list = await dbContext.Set<Expense>()
            .ToListAsync(cancellationToken);
        
            
        var mappedList = mapper.Map<List<Expense>, List<ExpenseResponse>>(list);
        return new List<ExpenseResponse>(mappedList);
    }

    public async Task<ExpenseResponse> Handle(GetExpenseByIdQuery request, CancellationToken cancellationToken)
    {
        await validate.IdGreaterThanZeroAsync(request.ExpenseRequestId, cancellationToken);

        var entity = await dbContext.Set<Expense>()
            .FirstOrDefaultAsync(x => x.ExpenseRequestId == request.ExpenseRequestId, cancellationToken);

        if (entity == null)
        {
            throw new Exception("Record not found");
        }

        var mapped = mapper.Map<Expense, ExpenseResponse>(entity);
        return mapped;
    }
    
    public async Task<List<ExpenseResponse>> Handle(GetExpenseByParameterQuery request, CancellationToken cancellationToken)
    {
        (string role, int creatorId)= await validate.UserAuthAsync(request.Model.UserId, cancellationToken);
        
        var predicate = PredicateBuilder.New<Expense>(true);
        
        if (request.Model.UserId is not null)
            predicate.And(x => x.UserId == request.Model.UserId);
            
        if (request.Model.CategoryId is not null)
            predicate.And(x => x.CategoryId == request.Model.CategoryId); 
            
        if (!string.IsNullOrEmpty(request.Model.Status))
            predicate.And(x => x.Status == (ExpenseRequestStatus)Enum.Parse(typeof(ExpenseRequestStatus), request.Model.Status));
            
        if (!string.IsNullOrEmpty(request.Model.PaymentStatus))
            predicate.And(x => x.PaymentStatus == (PaymentRequestStatus)Enum.Parse(typeof(PaymentRequestStatus), request.Model.PaymentStatus));
        
        var list =  await dbContext.Set<Expense>()
            .Where(predicate).ToListAsync(cancellationToken);
        
        var mapped = mapper.Map<List<Expense>, List<ExpenseResponse>>(list);
        return mapped;

    }
}


/*
 *
 *
 *         var predicate = PredicateBuilder.New<Expense>(true);
   predicate.And(x => x.ExpenseRequestId == fromdb.ExpenseRequestId);
   predicate.And(x => x.Status == ExpenseRequestStatus.Pending);
   
   var expense = await validate.RecordNotExistAsync<Expense>(predicate, cancellationToken);
 */