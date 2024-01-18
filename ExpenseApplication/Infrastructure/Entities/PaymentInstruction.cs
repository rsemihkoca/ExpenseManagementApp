using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Business.Entities;

[Table("PaymentInstruction", Schema = "dbo")]
public class PaymentInstruction : BaseEntity
{
    public int PaymentInstructionId { get; set; } // (Primary Key)
    public int ExpenseRequestId { get; set; } // (Foreign Key to Expense)
    public string PaymentStatus { get; set; } // (Pending, Completed)
    public DateTime? PaymentDate { get; set; }
    // Additional payment-related fields

    // Navigation property
    public virtual Expense Expense { get; set; }
}

public class PaymentInstructionConfiguration : IEntityTypeConfiguration<PaymentInstruction>
{
    public void Configure(EntityTypeBuilder<PaymentInstruction> builder)
    {
        builder.HasKey(p => p.PaymentInstructionId);
        builder.HasIndex(p => p.PaymentInstructionId).IsUnique();
        
        builder.Property(p => p.PaymentStatus).IsRequired().HasMaxLength(255);
        // Additional configuration for payment instruction entity
        
        builder.HasOne(p => p.Expense)
            .WithOne(e => e.PaymentInstruction)
            .HasForeignKey<PaymentInstruction>(p => p.ExpenseRequestId);
            // .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of Expense if it is referenced by a PaymentInstruction
    }
}