using Business.Entities;

namespace Infrastructure.Dtos;

public class ReportFrequencyRequest
{
    public string Type { get; set; }
}

public class PersonnelSummaryRequest
{
    public string Type { get; set; }
    public int UserId { get; set; }
}

public class ApprovedPaymentFrequencyReportResponse
{
    public string Type { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public int ApprovedCount { get; set; }
    public double ApprovedSum { get; set; }
    public double AverageApprovedAmount { get; set; }
}

public class RejectedPaymentFrequencyReportResponse
{
    public string Type { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public int RejectedCount { get; set; }
    public double RejectedSum { get; set; }
    public double AverageRejectedAmount { get; set; }
}

public class PersonnelExpenseFrequency
{
    public int UserId { get; set; }
    public string FullName { get; set; }
    public double PendingCount { get; set; }
    public double PendingSum { get; set; }
    public double AveragePendingAmount { get; set; }
}


public class PersonnelExpenseFrequencyReportResponse
{
    public string Type { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public int TotalPendingCount { get; set; }
    public double TotalPendingSum { get; set; }
    public double AveragePendingAmount { get; set; }
    public List<PersonnelExpenseFrequency> PersonnelExpenseFrequencies { get; set; }
}



public class PersonnelSummaryReportResponse
{
    public int UserId { get; set; }
    public int TotalCount { get; set; }
    public int ApprovedCount { get; set; }
    public int RejectedCount { get; set; }
    public int PendingCount { get; set; }
    
    public string ApprovedPercentage { get; set; }
    public double ApprovedSum { get; set; }
    public double RejectedSum { get; set; }
    public double PendingSum { get; set; }

    public List<ExpenseResponse> Expenses { get; set; }
}