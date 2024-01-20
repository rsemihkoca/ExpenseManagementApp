using Application.Cqrs;
using Infrastructure.Dtos;
using MediatR;

namespace Application.Commands;

public class ExpenseCategoryCommandHandler :
    IRequestHandler<CreateExpenseCategoryCommand, ExpenseCategoryResponse>,
    IRequestHandler<UpdateExpenseCategoryCommand, ExpenseCategoryResponse>,
    IRequestHandler<DeleteExpenseCategoryCommand, ExpenseCategoryResponse>
{
    public async Task<ExpenseCategoryResponse> Handle(CreateExpenseCategoryCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<ExpenseCategoryResponse> Handle(UpdateExpenseCategoryCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<ExpenseCategoryResponse> Handle(DeleteExpenseCategoryCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
