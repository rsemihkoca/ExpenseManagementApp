using Application.Cqrs;
using Infrastructure.Data.DbContext;
using Infrastructure.Dtos;
using Infrastructure.Token;
using MediatR;
using Microsoft.Extensions.Options;

namespace Application.Commands;

public class ExpenseCommandHandler :
    IRequestHandler<CreateExpenseCommand, ExpenseResponse>
{
    private readonly ExpenseDbContext dbContext;
    private readonly JwtConfig jwtConfig;

    public ExpenseCommandHandler(ExpenseDbContext dbContext, IOptionsMonitor<JwtConfig> jwtConfig)
    {
        this.dbContext = dbContext;
        this.jwtConfig = jwtConfig.CurrentValue;
    }


    public async Task<ExpenseResponse> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        
        // category validation user validation amount validation 
        // check float category id will be casted or what
        // cast category name to id 
        
        /* personel sadece kendisi için expense oluşturabilir
         * admin herkes için expense oluşturabilir
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         */
        return await Task.FromResult(new ExpenseResponse());
    }
}