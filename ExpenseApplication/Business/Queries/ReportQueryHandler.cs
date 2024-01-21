using Application.Cqrs;
using Application.Validators;
using AutoMapper;
using Business.Entities;
using Business.Enums;
using Infrastructure.Data.DbContext;
using Infrastructure.Dtos;
using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries;

public class ReportQueryHandler :
    IRequestHandler<ApprovedPaymentFrequencyReport, ApprovedPaymentFrequencyReportResponse>,
    IRequestHandler<RejectedPaymentFrequencyReport, RejectedPaymentFrequencyReportResponse>,
    IRequestHandler<PersonnelExpenseFrequencyReport, PersonnelExpenseFrequencyReportResponse>,
    IRequestHandler<PersonnelSummaryReport, PersonnelSummaryReportResponse>
{
    private readonly ExpenseDbContext dbContext;
    private readonly IMapper mapper;
    private readonly IHandlerValidator validate;

    public ReportQueryHandler(ExpenseDbContext dbContext, IMapper mapper, IHandlerValidator validate)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
        this.validate = validate;
    }


    public async Task<ApprovedPaymentFrequencyReportResponse> Handle(ApprovedPaymentFrequencyReport request,
        CancellationToken cancellationToken)
    {
        /* get all approved count and sum in CalculatedStartEndDate */
        var (startDate, endDate) = CalculateStartEndDate(request.Model.Type);

        var predicate = PredicateBuilder.New<Expense>(true);

        predicate = predicate.And(x => x.Status == ExpenseRequestStatus.Approved)
                                .And(x => x.CreationDate >= startDate && x.CreationDate <= endDate);

        var query = dbContext.Set<Expense>().Where(predicate);

        var count = await query.CountAsync(cancellationToken);
        var sum = await query.SumAsync(e => e.Amount, cancellationToken);

        ApprovedPaymentFrequencyReportResponse response = new()
        {
            Type = request.Model.Type,
            StartDate = startDate.ToString("dd/MM/yyyy HH:mm:ss"),
            EndDate = endDate.ToString("dd/MM/yyyy HH:mm:ss"),
            ApprovedCount = count,
            ApprovedSum = sum,
            AverageApprovedAmount = count == 0 ? 0 : sum / count
        };

        return response;
    }

    public async Task<RejectedPaymentFrequencyReportResponse> Handle(RejectedPaymentFrequencyReport request,
        CancellationToken cancellationToken)
    {
        /* get all rejected count and sum in CalculatedStartEndDate */
        var (startDate, endDate) = CalculateStartEndDate(request.Model.Type);

        var predicate = PredicateBuilder.New<Expense>(true);

        predicate = predicate.And(x => x.Status == ExpenseRequestStatus.Rejected)
                                .And(x => x.CreationDate >= startDate && x.CreationDate <= endDate);

        var query = dbContext.Set<Expense>().Where(predicate);

        var count = await query.CountAsync(cancellationToken);
        var sum = await query.SumAsync(e => e.Amount, cancellationToken);

        RejectedPaymentFrequencyReportResponse response = new()
        {
            Type = request.Model.Type,
            StartDate = startDate.ToString("dd/MM/yyyy HH:mm:ss"),
            EndDate = endDate.ToString("dd/MM/yyyy HH:mm:ss"),
            RejectedCount = count,
            RejectedSum = sum,
            AverageRejectedAmount = count == 0 ? 0 : sum / count
        };

        return response;
    }

    public async Task<PersonnelExpenseFrequencyReportResponse> Handle(PersonnelExpenseFrequencyReport request,
        CancellationToken cancellationToken)
    {
        /* get all pending count and sum in CalculatedStartEndDate group by user id*/
        var (startDate, endDate) = CalculateStartEndDate(request.Model.Type);

        var predicate = PredicateBuilder.New<Expense>(true);

        predicate = predicate.And(x => x.Status == ExpenseRequestStatus.Pending)
                                .And(x => x.CreationDate >= startDate && x.CreationDate <= endDate);

        var query = dbContext.Set<Expense>()
                                    .Include(x => x.User)
                                    .Where(predicate);

        var count = await query.CountAsync(cancellationToken);
        var sum = await query.SumAsync(e => e.Amount, cancellationToken);
        var personallList = await query
                            .GroupBy(e => e.UserId)
                            .Select(g => new PersonnelExpenseFrequency
                            {
                                UserId = g.Key,
                                FullName = g.FirstOrDefault().User.FirstName + " " + g.FirstOrDefault().User.LastName,
                                PendingCount = g.Count(),
                                PendingSum = g.Sum(e => e.Amount),
                                AveragePendingAmount = g.Count() == 0 ? 0 : g.Sum(e => e.Amount) / g.Count()
                            }).ToListAsync(cancellationToken);

        return new PersonnelExpenseFrequencyReportResponse
        {
            Type = request.Model.Type,
            StartDate = startDate.ToString("dd/MM/yyyy HH:mm:ss"),
            EndDate = endDate.ToString("dd/MM/yyyy HH:mm:ss"),
            TotalPendingCount = count,
            TotalPendingSum = sum,
            AveragePendingAmount = count == 0 ? 0 : sum / count,
            PersonnelExpenseFrequencies = personallList
        };
    }

    public async Task<PersonnelSummaryReportResponse> Handle(PersonnelSummaryReport request,
        CancellationToken cancellationToken)
    {
        /* check if personnel can only see their own */
        
        (string role, int creatorId)= await validate.UserAuthAsync(request.Model.UserId, cancellationToken);
        
        var (startDate, endDate) = CalculateStartEndDate(request.Model.Type);

        var predicate = PredicateBuilder.New<Expense>(true);

        predicate = predicate.And(x => x.CreationDate >= startDate && x.CreationDate <= endDate)
                                .And(x => x.User.UserId == request.Model.UserId);

        var query = dbContext.Set<Expense>()
            .Include(x => x.User)
            .Where(predicate);
        
        int totalCount = await query.CountAsync(cancellationToken);
        int approvedCount = await query.CountAsync(e => e.Status == ExpenseRequestStatus.Approved, cancellationToken);
        int rejectedCount = await query.CountAsync(e => e.Status == ExpenseRequestStatus.Rejected, cancellationToken);
        int pendingCount = await query.CountAsync(e => e.Status == ExpenseRequestStatus.Pending, cancellationToken);
        double approvedPercentage = approvedCount == 0 ? 0 : (double)approvedCount / totalCount * 100;
        double approvedSum = await query.SumAsync(e => e.Amount, cancellationToken);
        double rejectedSum = await query.SumAsync(e => e.Amount, cancellationToken);
        double pendingSum = await query.SumAsync(e => e.Amount, cancellationToken);
        
        List<ExpenseResponse> expenses = await query
                                                    .Select(e => new ExpenseResponse
                                                    {
                                                        ExpenseRequestId = e.ExpenseRequestId,
                                                        CategoryId = e.CategoryId,
                                                        ExpenseStatus = EnumToString<ExpenseRequestStatus>(e.Status),
                                                        PaymentStatus = EnumToString<PaymentRequestStatus>(e.PaymentStatus),
                                                        PaymentDescription = e.PaymentDescription,
                                                        Amount = e.Amount,
                                                        PaymentMethod = e.PaymentMethod,
                                                        PaymentLocation = e.PaymentLocation,
                                                        Documents = e.Documents,
                                                        Description = e.Description,
                                                        CreationDate = e.CreationDate.ToString("dd/MM/yyyy HH:mm:ss"),
                                                        LastUpdateTime = e.LastUpdateTime.ToString("dd/MM/yyyy HH:mm:ss"),
                    

                                                    }).ToListAsync(cancellationToken);

        return new PersonnelSummaryReportResponse
        {
            UserId = request.Model.UserId,
            TotalCount = totalCount,
            ApprovedCount = approvedCount,
            RejectedCount = rejectedCount,
            PendingCount = pendingCount,
            ApprovedPercentage = (approvedPercentage).ToString("0") + "%",
            ApprovedSum = approvedSum,
            RejectedSum = rejectedSum,
            PendingSum = pendingSum,
            Expenses = expenses
            
        };

    }
    
    /*
     *
     * public class PersonnelSummaryReportResponse
       {
           public int UserId { get; set; }
           public string FullName { get; set; }
           public int TotalCount { get; set; }
           public int ApprovedCount { get; set; }
           public int RejectedCount { get; set; }
           public int PendingCount { get; set; }
           
           public int ApprovedPercentage { get; set; }
           public double ApprovedSum { get; set; }
           public double RejectedSum { get; set; }
           public double PendingSum { get; set; }
       
           List<ExpenseResponse> Expenses { get; set; }
       }
     */

    private static (DateTime startDate, DateTime endDate) CalculateStartEndDate(string frequency)
    {
        DateTime inputDate = DateTime.Now;

        DateTime startDate = frequency.ToLower() switch
        {
            "daily" => new DateTime(inputDate.Year, inputDate.Month, inputDate.Day, 0, 0, 0),
            "weekly" => StartOfWeek(inputDate),
            "monthly" => new DateTime(inputDate.Year, inputDate.Month, 1, 0, 0, 0),
            _ => throw new ArgumentException("Invalid frequency. Supported values are daily, weekly, or monthly.")
        };

        DateTime endDate = frequency.ToLower() switch
        {
            "daily" => new DateTime(inputDate.Year, inputDate.Month, inputDate.Day, 23, 59, 59).AddMilliseconds(-1),
            "weekly" => startDate.AddDays(6).AddDays(1).AddMilliseconds(-1),
            "monthly" => startDate.AddMonths(1).AddMilliseconds(-1),
            _ => throw new ArgumentException("Invalid frequency. Supported values are daily, weekly, or monthly.")
        };

        return (startDate, endDate);
    }

    private static DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek = DayOfWeek.Monday)
    {
        int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
        return dt.AddDays(-1 * diff).Date;
    }

    // private static T ParseEnum<T>(string value) where T : struct, Enum
    // {
    //     return Enum.Parse<T>(value, true);
    // }
    
    private static string EnumToString<T>(T value) where T : struct, Enum
    {
        return Enum.GetName(typeof(T), value);
    }
}

