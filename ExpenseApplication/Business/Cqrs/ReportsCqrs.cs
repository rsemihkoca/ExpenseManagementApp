using Business.Enums;
using Infrastructure.Dtos;
using MediatR;

namespace Application.Cqrs;

// all of them daily, weekly, monthly
//  admin
public record ApprovedPaymentFrequencyReport(ReportFrequencyRequest Model) : IRequest<ApprovedPaymentFrequencyReportResponse>;
public record RejectedPaymentFrequencyReport(ReportFrequencyRequest Model) : IRequest<RejectedPaymentFrequencyReportResponse>;

//  admin
// group by user id
public record PersonnelExpenseFrequencyReport(ReportFrequencyRequest Model) : IRequest<PersonnelExpenseFrequencyReportResponse>;

// personnel and admin
// personnel can only see their own
public record PersonnelSummaryReport(PersonnelSummaryRequest Model) : IRequest<PersonnelSummaryReportResponse>;
