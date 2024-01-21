using Business.Enums;

namespace Infrastructure.Dtos;

public class CreatePaymentInstructionRequest
{
    public int ExpenseRequestId { get; set; }
    public DateTime? PaymentDate { get; set; }
    public string PaymentDescription { get; set; }
}

public class UpdatePaymentInstructionRequest
{
    public string PaymentStatus { get; set; }
    public DateTime? PaymentDate { get; set; }
    public string PaymentDescription { get; set; }
}

public class PaymentInstructionResponse
{
    public double Amount { get; set; }
    public string Description { get; set; }
    public string ExpenseStatus { get; set; }
    
    public int ExpenseRequestId { get; set; }
    public string PaymentStatus { get; set; }
    public string PaymentDate { get; set; }
    public string PaymentDescription { get; set; }
}