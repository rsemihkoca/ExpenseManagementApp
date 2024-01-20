using Application.Cqrs;
using Application.Validators;
using AutoMapper;
using Business.Entities;
using Infrastructure.Data.DbContext;
using Infrastructure.Dtos;
using MediatR;

namespace Application.Commands;

public class OldExpenseCategoryCommandHandler :
    IRequestHandler<CreateExpenseCategoryCommand, ExpenseCategoryResponse>,
    IRequestHandler<UpdateExpenseCategoryCommand, ExpenseCategoryResponse>,
    IRequestHandler<DeleteExpenseCategoryCommand, ExpenseCategoryResponse>
{
    private readonly ExpenseDbContext dbContext;
    private readonly IMapper mapper;
    private readonly IHandlerValidator validate;

    public OldExpenseCategoryCommandHandler(
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
        // await validator.ValidateCategoryNameNotExistAsync(request.Model.CategoryName, cancellationToken);
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
        // var fromdb =
        //     await validator.ValidateCategoryIsExistAsync(request.CategoryId, cancellationToken);
        // await validator.ValidateCategoryNameNotExistAsync(request.Model.CategoryName, cancellationToken);
        var fromdb = await validate.RecordExistAsync<ExpenseCategory>(x => x.CategoryId == request.CategoryId, cancellationToken);
        await validate.RecordNotExistAsync<ExpenseCategory>(x => x.CategoryName == request.Model.CategoryName, cancellationToken);

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
        // var fromdb = await validator.ValidateCategoryIsExistAsync(request.CategoryId, cancellationToken);
        
        var fromdb = await validate.RecordExistAsync<ExpenseCategory>(x => x.CategoryId == request.CategoryId, cancellationToken);

        await validate.ActiveExpenseNotExistAsync(request.CategoryId,
            expense => expense.CategoryId == request.CategoryId, cancellationToken);

        dbContext.Remove(fromdb);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new ExpenseCategoryResponse()
        {
            CategoryId = fromdb.CategoryId,
            CategoryName = fromdb.CategoryName
        };
    }
}