using Business.Cqrs;
using Business.Validators;
using AutoMapper;
using Infrastructure.DbContext;
using Infrastructure.Entities;
using MediatR;
using Schemes.Dtos;
using Schemes.Enums;

namespace Business.Commands;

public class ExpenseCategoryCommandHandler :
    IRequestHandler<CreateExpenseCategoryCommand, ExpenseCategoryResponse>,
    IRequestHandler<UpdateExpenseCategoryCommand, ExpenseCategoryResponse>,
    IRequestHandler<DeleteExpenseCategoryCommand, ExpenseCategoryResponse>
{
    private readonly ExpenseDbContext dbContext;
    private readonly IMapper mapper;
    private readonly IHandlerValidator validate;

    public ExpenseCategoryCommandHandler(
        ExpenseDbContext dbContext,
        IMapper mapper,
        IHandlerValidator validator)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
        this.validate = validator;
    }

    public async Task<ExpenseCategoryResponse> Handle(CreateExpenseCategoryCommand request,
        CancellationToken cancellationToken)
    {
        await validate.RecordNotExistAsync<ExpenseCategory>(x => x.CategoryName == request.Model.CategoryName, cancellationToken);

        ExpenseCategory entity = new ExpenseCategory
        {
            CategoryName = request.Model.CategoryName,
        };

        var entityResult = await dbContext.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var mapped = mapper.Map<ExpenseCategory, ExpenseCategoryResponse>(entityResult.Entity);
        return mapped;
    }

    public async Task<ExpenseCategoryResponse> Handle(UpdateExpenseCategoryCommand request,
        CancellationToken cancellationToken)
    {
        /* check if category exist
         * check if category name already exist
         */
        await validate.IdGreaterThanZeroAsync(request.CategoryId, cancellationToken);
        var fromdb = await validate.RecordExistAsync<ExpenseCategory>(x => x.CategoryId == request.CategoryId, cancellationToken);
        await validate.RecordNotExistAsync<ExpenseCategory>(x => x.CategoryName == request.Model.CategoryName.ToUpper(), cancellationToken);

        fromdb.CategoryName = request.Model.CategoryName;
        await dbContext.SaveChangesAsync(cancellationToken);

        return new ExpenseCategoryResponse()
        {
            CategoryId = fromdb.CategoryId,
            CategoryName = fromdb.CategoryName
        };
    }

    public async Task<ExpenseCategoryResponse> Handle(DeleteExpenseCategoryCommand request,
        CancellationToken cancellationToken)
    {
        /* check if category exist
         * check if pending expense exist with this category
         */
        await validate.IdGreaterThanZeroAsync(request.CategoryId, cancellationToken);

        var fromdb = await validate.RecordExistAsync<ExpenseCategory>(x => x.CategoryId == request.CategoryId, cancellationToken);

        var pendingExpense = await validate.RecordNotExistAsync<Expense>(x => x.CategoryId == request.CategoryId && x.Status == ExpenseRequestStatus.Pending, cancellationToken);

        dbContext.Remove(fromdb);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new ExpenseCategoryResponse()
        {
            CategoryId = fromdb.CategoryId,
            CategoryName = fromdb.CategoryName
        };
    }
}