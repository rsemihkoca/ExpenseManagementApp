using Business.Enums;

namespace Business.Entities;


public class Expense
{
    public int ExpenseRequestID { get; set; }
    public int UserID { get; set; }
    public decimal Amount { get; set; }
    public int CategoryID { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentLocation { get; set; }
    public string Documents { get; set; }
    public ExpenseRequestStatus Status { get; set; }
    public string RejectionReason { get; set; }
    public DateTime Timestamp { get; set; }

    // Navigation properties
    public virtual User User { get; set; }
    public virtual ExpenseCategory ExpenseCategory { get; set; }
    public virtual PaymentInstruction PaymentInstruction { get; set; }
}