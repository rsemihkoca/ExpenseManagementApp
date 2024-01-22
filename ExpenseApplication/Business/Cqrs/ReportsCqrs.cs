using Infrastructure.Dtos;
using MediatR;

namespace Business.Cqrs;

public record ApprovedPaymentFrequencyReport(ReportFrequencyRequest Model) : IRequest<ApprovedPaymentFrequencyReportResponse>;
public record RejectedPaymentFrequencyReport(ReportFrequencyRequest Model) : IRequest<RejectedPaymentFrequencyReportResponse>;

public record PersonnelExpenseFrequencyReport(ReportFrequencyRequest Model) : IRequest<PersonnelExpenseFrequencyReportResponse>;

public record PersonnelSummaryReport(PersonnelSummaryRequest Model) : IRequest<PersonnelSummaryReportResponse>;
