using Application.Cqrs;
using Application.Services;
using Application.Validators;
using AutoMapper;
using Business.Entities;
using Infrastructure.Data.DbContext;
using Infrastructure.Dtos;
using Infrastructure.Exceptions;
using MediatR;

namespace Application.Commands;

public class ExpenseCommandHandler :
    IRequestHandler<CreateExpenseCommand, ExpenseResponse>,
    IRequestHandler<UpdateExpenseCommand, ExpenseResponse>
{
    private readonly ExpenseDbContext dbContext;
    private readonly IUserService userService;
    private readonly IMapper mapper;
    private readonly IHandlerValidator validate;

    public ExpenseCommandHandler(ExpenseDbContext dbContext, IMapper mapper, IHandlerValidator validator,
        IUserService userService)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
        this.validate = validator;
        this.userService = userService;
    }


    public async Task<ExpenseResponse> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        await validate.RecordExistAsync<User>(x => x.UserId == request.Model.UserId, cancellationToken);
        await validate.RecordExistAsync<ExpenseCategory>(x => x.CategoryId == request.Model.CategoryId,
            cancellationToken);

        var role = userService.GetUserRole();
        var creatorId = userService.GetUserId();
        switch (role)
        {
            case "Admin":
                break;
            case "Personnel":
                if (request.Model.UserId != userService.GetUserId())
                {
                    throw new HttpException("You can't create expense for other users", 403);
                }
                break;
            default:
                throw new HttpException("You can't create expense", 403);
        }

        var entity = mapper.Map<CreateExpenseRequest, Expense>(request.Model);
        entity.CreatedBy = creatorId;

        var entityResult = await dbContext.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var mapped = mapper.Map<Expense, ExpenseResponse>(entityResult.Entity);
        return mapped;
    }

    public async Task<ExpenseResponse> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
    {
        // admin kendini approve edemez basşka bir admin approve edebilir ?
        // Admin approve ederse payment instruction oluşturulur
        // approve yapıldıgında backfire, reject yapıldıgında backfire
        // UpdateRequest için validator eklemeyi unutma !!!! DONE
        // must be valid enum dene

        return new ExpenseResponse();
    }
}

/*
 *
 *        /* eger fromdb.PaymentRequestStatus Completed degil ve request.Model Completed ise jobı çalıştır.* / 
   // Start Payment Service
   
   expense.LastActivityTime = DateTime.Now;
   (decimal amount, string fromIban, string toIban) = (100, "TR123456789", "TR987654321");
   var jobId = BackgroundJob.Enqueue(() => payment.ProcessPayment(amount, fromIban, toIban));
   // First update payment instruction status to processing
   BackgroundJob.ContinueJobWith(jobId, () => Console.WriteLine("Continuation!"));
 * 
 */