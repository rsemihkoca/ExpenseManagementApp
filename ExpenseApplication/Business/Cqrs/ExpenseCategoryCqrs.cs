using MediatR;

namespace Application.Cqrs;

public record CreateExpenseCategoryCommand(CreateExpenseCategoryRequest Model) : IRequest<ExpenseCategoryResponse>;
public record UpdateExpenseCategoryCommand(int CategoryId,CreateExpenseCategoryRequest Model) : IRequest<ExpenseCategoryResponse>;
public record DeleteExpenseCategoryCommand(int CategoryId) : IRequest<ExpenseCategoryResponse>;

public record GetAllExpenseCategoryQuery() : IRequest<List<ExpenseCategoryResponse>>;
public record GetExpenseCategoryByIdQuery(int CategoryId) : IRequest<ExpenseCategoryResponse>;