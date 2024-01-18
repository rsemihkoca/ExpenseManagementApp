using Business.Enums;

namespace Infrastructure.Dtos;

public class InsertExpenseRequest
{
    // public int ExpenseRequestId { get; set; }
    // public int UserId { get; set; } from JWT
    public double Amount { get; set; }
    public int CategoryId { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentLocation { get; set; }

    public string Documents { get; set; }

    // public ExpenseRequestStatus Status { get; set; } // (Pending
    public string Description { get; set; }
    // public DateTime CreationDate { get; set; }
    // public DateTime LastUpdateTime { get; set; }
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
    public int PersonnelName { get; set; } // (Foreign Key)
    public int CategoryName { get; set; } // (Foreign Key to ExpenseCategory)
    public ExpenseRequestStatus ExpenseStatus { get; set; }
    public string PaymentStatus { get; set; }

    public double Amount { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentLocation { get; set; }
    public string Documents { get; set; }
    public string Description { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastUpdateTime { get; set; }
}