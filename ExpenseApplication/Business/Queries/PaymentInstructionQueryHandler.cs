using Application.Cqrs;
using Application.Validators;
using AutoMapper;
using Business.Entities;
using Infrastructure.Data.DbContext;
using Infrastructure.Dtos;
using Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries;
public class PaymentInstructionQueryHandler :
    IRequestHandler<GetAllPaymentInstructionQuery,List<PaymentInstructionResponse>>,
    IRequestHandler<GetPaymentInstructionByIdQuery,PaymentInstructionResponse>
{

    private readonly ExpenseDbContext dbContext;
    private readonly IMapper mapper;

    public PaymentInstructionQueryHandler(ExpenseDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }
    public async Task<List<PaymentInstructionResponse>> Handle(GetAllPaymentInstructionQuery request, CancellationToken cancellationToken)
    {
        var list = await dbContext.Set<PaymentInstruction>()
            .Include(x => x.Expense)
            .ToListAsync(cancellationToken);
            
        if (list.Count == 0)
        {
            throw new HttpException("No record found", 404);
        }
        
        var mappedList = mapper.Map<List<PaymentInstruction>, List<PaymentInstructionResponse>>(list);
        return mappedList;
    }

    public async Task<PaymentInstructionResponse> Handle(GetPaymentInstructionByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Set<PaymentInstruction>()
            .Include(x => x.Expense)
            .FirstOrDefaultAsync(x => x.PaymentInstructionId == request.PaymentInstructionId, cancellationToken);
            
        if (entity == null)
        {
            throw new HttpException($"Record {request.PaymentInstructionId} not found", 404);
        }
        
        var mapped = mapper.Map<PaymentInstruction, PaymentInstructionResponse>(entity);
        return mapped;
    }
}