using System.ComponentModel.DataAnnotations.Schema;
using Business.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Business.Entities;

[Table("PaymentInstruction", Schema = "CaseDb")]
public class PaymentInstruction : BaseEntity
{
    public int PaymentInstructionId { get; set; } // (Primary Key)
    public int ExpenseRequestId { get; set; } // (Foreign Key to Expense)
    public PaymentRequestStatus PaymentStatus { get; set; } // (Pending, Completed, Failed)
    public DateTime? PaymentDate { get; set; }

    public virtual Expense Expense { get; set; }
}

public class PaymentInstructionConfiguration : IEntityTypeConfiguration<PaymentInstruction>
{
    public void Configure(EntityTypeBuilder<PaymentInstruction> builder)
    {
        builder.HasKey(p => p.PaymentInstructionId);
        builder.HasIndex(p => p.PaymentInstructionId).IsUnique();
        
        builder.Property(p => p.ExpenseRequestId).IsRequired();
        
        builder.Property(p => p.PaymentStatus).IsRequired().HasMaxLength(255).HasConversion<string>();
        
        builder.Property(p => p.PaymentDate).IsRequired();
        
        
        builder.HasOne(p => p.Expense)
            .WithOne(e => e.PaymentInstruction)
            .HasForeignKey<PaymentInstruction>(p => p.ExpenseRequestId);
            // .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of Expense if it is referenced by a PaymentInstruction
    }
}