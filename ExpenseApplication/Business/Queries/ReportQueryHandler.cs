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
    IRequestHandler<PersonnelSummaryReport, List<PersonnelSummaryReportResponse>>
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

        predicate = predicate.And(x => x.Status == ParseEnum<ExpenseRequestStatus>("Approved"))
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

        predicate = predicate.And(x => x.Status == ParseEnum<ExpenseRequestStatus>("Rejected"))
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

        predicate = predicate.And(x => x.Status == ParseEnum<ExpenseRequestStatus>("Pending"))
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

    public async Task<List<PersonnelSummaryReportResponse>> Handle(PersonnelSummaryReport request,
        CancellationToken cancellationToken)
    {
        (role, _) = await validate.ValidateUser(request.UserId);
    }

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

    public static T ParseEnum<T>(string value) where T : struct, Enum
    {
        return Enum.Parse<T>(value, true);
    }
}