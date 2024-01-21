using Application.Cqrs;
using Application.Services;
using Application.Validators;
using AutoMapper;
using Business.Entities;
using Business.Enums;
using Hangfire;
using Infrastructure.Data.DbContext;
using Infrastructure.Dtos;
using LinqKit;
using MediatR;

namespace Application.Commands;

public class PaymentInstructionCommandHandler :
    IRequestHandler<CreatePaymentInstructionCommand, PaymentInstructionResponse>,
    IRequestHandler<UpdatePaymentInstructionCommand, PaymentInstructionResponse>,
    IRequestHandler<DeletePaymentInstructionCommand, PaymentInstructionResponse>
{
    private readonly ExpenseDbContext dbContext;
    private readonly IMapper mapper;
    private readonly IHandlerValidator validate;
    private readonly IPaymentService payment;

    public PaymentInstructionCommandHandler(
        ExpenseDbContext dbContext,
        IMapper mapper,
        IHandlerValidator validator, IPaymentService payment)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
        this.validate = validator;
        this.payment = payment;
    }

    public async Task<PaymentInstructionResponse> Handle(CreatePaymentInstructionCommand request, CancellationToken cancellationToken)
    {
        /* check no previous payment instruction for this entity*/
        /* check there is expense for this payment instruction*/
        await validate.RecordNotExistAsync<PaymentInstruction>(x => x.ExpenseRequestId == request.Model.ExpenseRequestId, cancellationToken);
        await validate.RecordExistAsync<Expense>(x => x.ExpenseRequestId == request.Model.ExpenseRequestId, cancellationToken);
        
        var entity = mapper.Map<CreatePaymentInstructionRequest, PaymentInstruction>(request.Model);
        
        var entityResult = await dbContext.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var mapped = mapper.Map<PaymentInstruction, PaymentInstructionResponse>(entityResult.Entity);
        return mapped;

    }

    public async Task<PaymentInstructionResponse> Handle(UpdatePaymentInstructionCommand request, CancellationToken cancellationToken)
    {
        var fromdb = await validate.RecordExistAsync<PaymentInstruction>(x => x.PaymentInstructionId == request.PaymentInstructionId, cancellationToken);

        fromdb.PaymentStatus =
            (PaymentRequestStatus)Enum.Parse(typeof(PaymentRequestStatus), request.Model.PaymentStatus, true);
        fromdb.PaymentDate = request.Model.PaymentDate;
        fromdb.PaymentDescription = request.Model.PaymentDescription;
        
        
        await dbContext.SaveChangesAsync(cancellationToken);
        var mapped = mapper.Map<PaymentInstruction, PaymentInstructionResponse>(fromdb);
        return mapped;
    }

    public async Task<PaymentInstructionResponse> Handle(DeletePaymentInstructionCommand request, CancellationToken cancellationToken)
    {
        var fromdb = await validate.RecordExistAsync<PaymentInstruction>(x => x.PaymentInstructionId == request.PaymentInstructionId, cancellationToken);
        /* check if expense is in status pending */
        
        var predicate = PredicateBuilder.New<Expense>(true);
        predicate.And(x => x.ExpenseRequestId == fromdb.ExpenseRequestId);
        predicate.And(x => x.Status == ExpenseRequestStatus.Pending);
        
        var expense = await validate.RecordNotExistAsync<Expense>(predicate, cancellationToken);
        
        dbContext.Remove(fromdb);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        var mapped = mapper.Map<PaymentInstruction, PaymentInstructionResponse>(fromdb);
        return mapped;
    }
}