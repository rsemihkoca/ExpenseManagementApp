using Infrastructure.Dtos;
using MediatR;

namespace Application.Cqrs;


// common endpoint for all entites

public record CreateExpenseCommand(CreateExpenseRequest Model) : IRequest<ExpenseResponse>;

public record UpdateExpenseCommand(int ExpenseRequestId, UpdateExpenseRequest Model) : IRequest<ExpenseResponse>;
// admin kendini onaylayamaz
public record DeleteExpenseCommand(int ExpenseRequestId) : IRequest<ExpenseResponse>;





public record GetAllExpenseQuery() : IRequest<List<ExpenseResponse>>;

public record GetExpenseByIdQuery(int ExpenseRequestId) : IRequest<ExpenseResponse>;

// add additional query to get expense by parameter

// public record GetExpenseByParameterQuery(string FirstName, string LastName, string IdentityNumber)
//     : IRequest<List<ExpenseResponse>>;