using Business.Enums;

namespace Infrastructure.Dtos;

public class GetExpenseByParameterRequest
{
    public int? UserId { get; set; }
    public int? CategoryId { get; set; }
    public string? Status { get; set; } // (Pending, Approved, Rejected)
    public string? PaymentStatus { get; set; } // (Pending, Declined, Completed, Failed)
}


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
    public string Status { get; set; }
    public string Description { get; set; }
    public string PaymentStatus { get; set; }
    public string PaymentDescription { get; set; }
}

public class ExpenseResponse
{
    public int ExpenseRequestId { get; set; } // (Primary Key)
    public int UserId { get; set; } // (Foreign Key)
    public int CategoryId { get; set; } // (Foreign Key to ExpenseCategory)
    public string ExpenseStatus { get; set; }
    public string PaymentStatus { get; set; }
    
    public string PaymentDescription { get; set; }

    public double Amount { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentLocation { get; set; }
    public string Documents { get; set; }
    public string Description { get; set; }
    public string CreationDate { get; set; }
    public string LastUpdateTime { get; set; }
}