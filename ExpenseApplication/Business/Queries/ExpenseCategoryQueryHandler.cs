using Business.Cqrs;
using AutoMapper;
using Business.Validators;
using Infrastructure.DbContext;
using Infrastructure.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Schemes.Dtos;
using Schemes.Exceptions;

namespace Business.Queries;

public class ExpenseCategoryQueryHandler :
    IRequestHandler<GetAllExpenseCategoryQuery,List<ExpenseCategoryResponse>>,
    IRequestHandler<GetExpenseCategoryByIdQuery,ExpenseCategoryResponse>
{

    private readonly ExpenseDbContext dbContext;
    private readonly IMapper mapper;
    private readonly IHandlerValidator validate;

    public ExpenseCategoryQueryHandler(ExpenseDbContext dbContext, IMapper mapper, IHandlerValidator validate)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
        this.validate = validate;
    }
    public async Task<List<ExpenseCategoryResponse>> Handle(GetAllExpenseCategoryQuery request, CancellationToken cancellationToken)
    {
        var list = await dbContext.Set<ExpenseCategory>().ToListAsync(cancellationToken);
        
        if (list.Count == 0)
        {
            throw new HttpException(Constants.ErrorMessages.NoRecordFound, 404);
        }
        
        var mappedList = mapper.Map<List<ExpenseCategory>, List<ExpenseCategoryResponse>>(list);
        return mappedList;
    }

    public async Task<ExpenseCategoryResponse> Handle(GetExpenseCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        await validate.IdGreaterThanZeroAsync(request.CategoryId, cancellationToken);

        var entity = await dbContext.Set<ExpenseCategory>().FirstOrDefaultAsync(x => x.CategoryId == request.CategoryId, cancellationToken);
        
        if (entity == null)
        {
            throw new HttpException($"Record {request.CategoryId} not found", 404);
        }
        
        var mapped = mapper.Map<ExpenseCategory, ExpenseCategoryResponse>(entity);
        return mapped;
    }
}
