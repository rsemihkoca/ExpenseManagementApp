using Application.Cqrs;
using AutoMapper;
using Business.Entities;
using Infrastructure.Data.DbContext;
using Infrastructure.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries;

public class ExpenseQueryHandler :
    IRequestHandler<GetAllExpenseQuery, List<ExpenseResponse>>,
    IRequestHandler<GetExpenseByIdQuery, ExpenseResponse>

{
    private readonly ExpenseDbContext dbContext;
    private readonly IMapper mapper;

    public ExpenseQueryHandler(ExpenseDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    //
    // public async Task<List<ExpenseResponse>> Handle(GetAllCustomerQuery request,
    //     CancellationToken cancellationToken)
    // {
    //     var list = await dbContext.Set<Customer>()
    //         .Include(x => x.Accounts)
    //         .Include(x => x.Contacts)
    //         .Include(x => x.Addresses).ToListAsync(cancellationToken);
    //     
    //     var mappedList = mapper.Map<List<Customer>, List<CustomerResponse>>(list);
    //      return new List<CustomerResponse>>(mappedList);
    // }
    //
    // public async Task<CustomerResponse>> Handle(GetCustomerByIdQuery request,
    //     CancellationToken cancellationToken)
    // {
    //     var entity =  await dbContext.Set<Customer>()
    //         .Include(x => x.Accounts)
    //         .Include(x => x.Contacts)
    //         .Include(x => x.Addresses)
    //         .FirstOrDefaultAsync(x => x.CustomerNumber == request.Id, cancellationToken);
    //
    //     if (entity == null)
    //     {
    //         return new CustomerResponse>("Record not found");
    //     }
    //     
    //     var mapped = mapper.Map<Customer, CustomerResponse>(entity);
    //     return new CustomerResponse>(mapped);
    // }
    //
    // public async Task<List<CustomerResponse>> Handle(GetCustomerByParameterQuery request,
    //     CancellationToken cancellationToken)
    // {
    //     var predicate = PredicateBuilder.New<Customer>(true);
    //     if (string.IsNullOrEmpty(request.FirstName))
    //         
    //         predicate.And(x => x.FirstName.ToUpper().Contains(request.FirstName.ToUpper()));
    //     if (string.IsNullOrEmpty(request.LastName))
    //         predicate.And(x => x.LastName.ToUpper().Contains(request.LastName.ToUpper()));
    //     
    //     if (string.IsNullOrEmpty(request.IdentityNumber))
    //         predicate.And(x => x.IdentityNumber.ToUpper().Contains(request.IdentityNumber.ToUpper()));
    //     
    //     var list =  await dbContext.Set<Customer>()
    //         .Include(x => x.Accounts)
    //         .Include(x => x.Contacts)
    //         .Include(x => x.Addresses)
    //         .Where(predicate).ToListAsync(cancellationToken);
    //     
    //     var mappedList = mapper.Map<List<Customer>, List<CustomerResponse>>(list);
    //     return new List<CustomerResponse>>(mappedList);
    public async Task<List<ExpenseResponse>> Handle(GetAllExpenseQuery request, CancellationToken cancellationToken)
    {
        var list = await dbContext.Set<Expense>()
            .Include(x => x.User)
            .Include(x => x.ExpenseCategory)
            .Include(x => x.PaymentInstruction)
            .ToListAsync(cancellationToken);
            
            /* creation_date and last_update_time is now */
            
    // public int ExpenseRequestId { get; set; } // (Primary Key)
        // public int PersonnelName { get; set; } // (Foreign Key)
    // public double Amount { get; set; }
        // public int CategoryName { get; set; } // (Foreign Key to ExpenseCategory)
    // public string PaymentMethod { get; set; }
    // public string PaymentLocation { get; set; }
    // public string Documents { get; set; }
    // public ExpenseRequestStatus Status { get; set; }
    // public string Description { get; set; }
        // public DateTime CreationDate { get; set; }
        // public DateTime LastUpdateTime { get; set; }
        // public string PaymentStatus { get; set; }
            
        var mappedList = mapper.Map<List<Expense>, List<ExpenseResponse>>(list);
        return new List<ExpenseResponse>(mappedList);
    }

    public async Task<ExpenseResponse> Handle(GetExpenseByIdQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}