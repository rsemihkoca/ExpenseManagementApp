using System.ComponentModel.DataAnnotations.Schema;
using Business.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Business.Entities;

[Table("Expense", Schema = "CaseDb")]
public class Expense : BaseEntity
{
    public int ExpenseRequestId { get; set; } // (Primary Key)
    public int UserId { get; set; } // (Foreign Key)
    
    public int CreatedBy { get; set; }
    public double Amount { get; set; }
    public int CategoryId { get; set; } // (Foreign Key to ExpenseCategory)
    public string PaymentMethod { get; set; }
    public string PaymentLocation { get; set; }
    public string Documents { get; set; } //  (File upload or references)
    public ExpenseRequestStatus Status { get; set; } // (Pending, Approved, Rejected)
    public string Description { get; set; } //  (from User)
    public DateTime CreationDate { get; set; }
    public DateTime LastUpdateTime { get; set; }
    public virtual User User { get; set; }
    public virtual ExpenseCategory ExpenseCategory { get; set; }
    public virtual PaymentInstruction PaymentInstruction { get; set; }
}

public class ExpenseRequestConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.HasKey(e => e.ExpenseRequestId);
        builder.HasIndex(e => e.ExpenseRequestId).IsUnique();
        
        builder.Property(e => e.UserId).IsRequired();
        
        builder.Property(e => e.CreatedBy).IsRequired();

        builder.Property(e => e.Amount).IsRequired();
        
        builder.Property(e => e.CategoryId).IsRequired();

        builder.Property(e => e.PaymentMethod).IsRequired().HasMaxLength(50);

        builder.Property(e => e.PaymentLocation).IsRequired().HasMaxLength(255);

        builder.Property(e => e.Documents).HasMaxLength(255); // Optional

        builder.Property(e => e.Status).IsRequired().HasConversion<string>();

        builder.Property(e => e.Description).IsRequired().HasMaxLength(255);

        builder.Property(e => e.CreationDate).IsRequired();
        
        builder.Property(e => e.LastUpdateTime).IsRequired();

        builder.HasOne(e => e.User)
            .WithMany(u => u.ExpenseRequests)
            .HasForeignKey(e => e.UserId);
            // .OnDelete(DeleteBehavior.Restrict); // Delete expense if user is deleted

        builder.HasOne(e => e.ExpenseCategory)
            .WithMany()
            .HasForeignKey(e => e.CategoryId);
            // .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of ExpenseCategory if it is referenced by an Expense

        builder.HasOne(e => e.PaymentInstruction)
            .WithOne(e => e.Expense)
            .HasForeignKey<PaymentInstruction>(p => p.ExpenseRequestId);

            // .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of PaymentInstruction if it is referenced by an Expense

        
    }
}