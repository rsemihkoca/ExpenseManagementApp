using Business.Cqrs;
using Business.Services;
using Business.Validators;
using AutoMapper;
using Hangfire;
using Infrastructure.DbContext;
using Infrastructure.Entities;
using MediatR;
using Schemes.Dtos;
using Schemes.Enums;

namespace Business.Commands;

public class ExpenseCommandHandler :
    IRequestHandler<CreateExpenseCommand, ExpenseResponse>,
    IRequestHandler<UpdateExpenseCommand, ExpenseResponse>,
    IRequestHandler<DeleteExpenseCommand, ExpenseResponse>,
    IRequestHandler<ApproveExpenseCommand, ExpenseResponse>,
    IRequestHandler<RejectExpenseCommand, ExpenseResponse>
{
    private readonly ExpenseDbContext dbContext;
    private readonly IMapper mapper;
    private readonly IHandlerValidator validate;
    private readonly IPaymentService payment;

    public ExpenseCommandHandler(ExpenseDbContext dbContext, IMapper mapper, IHandlerValidator validator, IPaymentService payment)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
        this.validate = validator;
        this.payment = payment;
    }


    public async Task<ExpenseResponse> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        (string role, int creatorId) = await validate.UserAuthAsync(request.Model.UserId, cancellationToken);

        await validate.RecordExistAsync<User>(x => x.UserId == request.Model.UserId, cancellationToken);
        await validate.RecordExistAsync<ExpenseCategory>(x => x.CategoryId == request.Model.CategoryId,
            cancellationToken);

        var entity = mapper.Map<CreateExpenseRequest, Expense>(request.Model);
        entity.CreatedBy = creatorId;

        var entityResult = await dbContext.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var mapped = mapper.Map<Expense, ExpenseResponse>(entityResult.Entity);
        return mapped;
    }

    public async Task<ExpenseResponse> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
    {
        // admin kendini approve edemez basşka bir admin approve edebilir ? Bundan vazgectim
        // Update approved bir sey tetiklemiyor sadece update ediyor elden verme durumları gibi
        // check if expense exist
        // check if new user exist, check if new category exist
        await validate.IdGreaterThanZeroAsync(request.ExpenseRequestId, cancellationToken);
        
        var fromdb = await validate.RecordExistAsync<Expense>(x => x.ExpenseRequestId == request.ExpenseRequestId,
            cancellationToken);
        await validate.RecordExistAsync<User>(x => x.UserId == request.Model.UserId, cancellationToken);
        await validate.RecordExistAsync<ExpenseCategory>(x => x.CategoryId == request.Model.CategoryId,
            cancellationToken);

        fromdb.UserId = request.Model.UserId;
        fromdb.Amount = request.Model.Amount;
        fromdb.CategoryId = request.Model.CategoryId;
        fromdb.PaymentMethod = request.Model.PaymentMethod;
        fromdb.PaymentLocation = request.Model.PaymentLocation;
        fromdb.Documents = request.Model.Documents;
        fromdb.Status = (ExpenseRequestStatus)Enum.Parse(typeof(ExpenseRequestStatus), request.Model.Status, true);
        fromdb.Description = request.Model.Description;
        fromdb.PaymentStatus =
            (PaymentRequestStatus)Enum.Parse(typeof(PaymentRequestStatus), request.Model.PaymentStatus, true);
        fromdb.PaymentDescription = request.Model.PaymentDescription;

        await dbContext.SaveChangesAsync(cancellationToken);
        var mapped = mapper.Map<Expense, ExpenseResponse>(fromdb);
        return mapped;
    }

    public async Task<ExpenseResponse> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
    {
        await validate.IdGreaterThanZeroAsync(request.ExpenseRequestId, cancellationToken);

        var fromdb = await validate.RecordExistAsync<Expense>(x => x.ExpenseRequestId == request.ExpenseRequestId,
            cancellationToken);

        dbContext.Remove(fromdb);
        await dbContext.SaveChangesAsync(cancellationToken);
        var mapped = mapper.Map<Expense, ExpenseResponse>(fromdb);
        return mapped;
    }

    public async Task<ExpenseResponse> Handle(ApproveExpenseCommand request, CancellationToken cancellationToken)
    {
        /*
         * check if expense exist
         * check if it is not approved and payment status not completed
         * do not check user exist since it cant be deleted if expense exist
         */
        await validate.IdGreaterThanZeroAsync(request.ExpenseRequestId, cancellationToken);


        var fromdb = await validate.RecordExistAsync<Expense>(x => x.ExpenseRequestId == request.ExpenseRequestId,
            cancellationToken);
        var awaiter = await validate.ExpenseCanBeApprovedAsync(fromdb, cancellationToken);
        
        fromdb.Status = ExpenseRequestStatus.Approved;
        fromdb.PaymentStatus = PaymentRequestStatus.OnProcess;
        fromdb.LastUpdateTime = DateTime.Now;
        
        await dbContext.SaveChangesAsync(cancellationToken);
        var mapped = mapper.Map<Expense, ExpenseResponse>(fromdb);
        
        double amount = fromdb.Amount;
        int fromUserId = fromdb.UserId;
        int toUserId = fromdb.CreatedBy;
        
        // Start Payment Service
        var jobId = BackgroundJob.Enqueue(() => payment.ProcessPayment(request.ExpenseRequestId, amount, fromUserId, toUserId));

        return mapped;
    }

    public async Task<ExpenseResponse> Handle(RejectExpenseCommand request, CancellationToken cancellationToken)
    {
        await validate.IdGreaterThanZeroAsync(request.ExpenseRequestId, cancellationToken);

        var fromdb = await validate.RecordExistAsync<Expense>(x => x.ExpenseRequestId == request.ExpenseRequestId,
            cancellationToken);
        await validate.ExpenseCanBeRejectedAsync(fromdb, cancellationToken);
        
        fromdb.Status = ExpenseRequestStatus.Rejected;
        fromdb.PaymentDescription = request.PaymentDescription ?? fromdb.PaymentDescription;
        fromdb.PaymentStatus = PaymentRequestStatus.Declined;
        fromdb.LastUpdateTime = DateTime.Now;
        await dbContext.SaveChangesAsync(cancellationToken);
        var mapped = mapper.Map<Expense, ExpenseResponse>(fromdb);
        return mapped;
    }
}
