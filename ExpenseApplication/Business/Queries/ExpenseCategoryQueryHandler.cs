using Application.Cqrs;
using AutoMapper;
using Business.Entities;
using Infrastructure.Data.DbContext;
using Infrastructure.Dtos;
using Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries;

public class ExpenseCategoryQueryHandler :
    IRequestHandler<GetAllExpenseCategoryQuery,List<ExpenseCategoryResponse>>,
    IRequestHandler<GetExpenseCategoryByIdQuery,ExpenseCategoryResponse>
{

    private readonly ExpenseDbContext dbContext;
    private readonly IMapper mapper;

    public ExpenseCategoryQueryHandler(ExpenseDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }
    public async Task<List<ExpenseCategoryResponse>> Handle(GetAllExpenseCategoryQuery request, CancellationToken cancellationToken)
    {
        var list = await dbContext.Set<ExpenseCategory>().ToListAsync(cancellationToken);
        
        if (list.Count == 0)
        {
            throw new HttpException("No record found", 404);
        }
        
        var mappedList = mapper.Map<List<ExpenseCategory>, List<ExpenseCategoryResponse>>(list);
        return mappedList;
    }

    public async Task<ExpenseCategoryResponse> Handle(GetExpenseCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Set<ExpenseCategory>().FirstOrDefaultAsync(x => x.CategoryId == request.CategoryId, cancellationToken);
        
        if (entity == null)
        {
            throw new HttpException($"Record {request.CategoryId} not found", 404);
        }
        
        var mapped = mapper.Map<ExpenseCategory, ExpenseCategoryResponse>(entity);
        return mapped;
    }
}
