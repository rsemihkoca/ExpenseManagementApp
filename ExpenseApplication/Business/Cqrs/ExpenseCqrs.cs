using Infrastructure.Dtos;
using MediatR;

namespace Application.Cqrs;


// common endpoint for all entites

public record CreateExpenseCommand(CreateExpenseRequest Model) : IRequest<ExpenseResponse>;

public record UpdateExpenseCommand(int ExpenseRequestId, UpdateExpenseRequest Model) : IRequest<ExpenseResponse>;
// admin kendini onaylayamaz
public record DeleteExpenseCommand(int ExpenseRequestId) : IRequest<ExpenseResponse>;

public record ApproveExpenseCommand(int ExpenseRequestId) : IRequest<ExpenseResponse>;

public record RejectExpenseCommand(int ExpenseRequestId) : IRequest<ExpenseResponse>;



public record GetAllExpenseQuery() : IRequest<List<ExpenseResponse>>;

public record GetExpenseByIdQuery(int ExpenseRequestId) : IRequest<ExpenseResponse>;
public record GetExpenseByParameterQuery(GetExpenseByParameterRequest Model) : IRequest<List<ExpenseResponse>>;
