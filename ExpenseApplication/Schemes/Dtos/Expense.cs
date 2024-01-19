using Business.Enums;

namespace Infrastructure.Dtos;

public class CreateExpenseRequest
{
    public int UserId { get; set; }
    public double Amount { get; set; }
    public int CategoryId { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentLocation { get; set; }

    public string Documents { get; set; }
    public string Description { get; set; }
}


public class UpdateExpenseRequest //     public int ExpenseRequestId { get; set; }
{
    public int UserId { get; set; }
    public double Amount { get; set; }
    public int CategoryId { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentLocation { get; set; }
    public string Documents { get; set; }
    public ExpenseRequestStatus Status { get; set; }
    public string Description { get; set; }
    public DateTime CreationDate { get; set; }
}

public class ExpenseResponse
{
    public int ExpenseRequestId { get; set; } // (Primary Key)
    public string PersonnelName { get; set; } // (Foreign Key)
    public string CategoryName { get; set; } // (Foreign Key to ExpenseCategory)
    public string ExpenseStatus { get; set; }

    public double Amount { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentLocation { get; set; }
    public string Documents { get; set; }
    public string Description { get; set; }
    public string CreationDate { get; set; }
    public string LastUpdateTime { get; set; }
}