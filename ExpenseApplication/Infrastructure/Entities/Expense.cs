using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Schemes.Enums;

namespace Infrastructure.Entities;

[Table(Constants.Database.ExpenseTable, Schema = Constants.Database.Schema)]
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
    public ExpenseRequestStatus Status { get; set; } = ExpenseRequestStatus.Pending; // (Pending, Approved, Rejected)
    public string Description { get; set; } //  (from User)
    public DateTime CreationDate { get; set; }
    public DateTime LastUpdateTime { get; set; }
    public PaymentRequestStatus PaymentStatus { get; set; } = PaymentRequestStatus.Pending;// (Pending, Declined, Completed, Failed)
    
    public DateTime? PaymentDate { get; set; } = null; // (if Status is Completed)
    public string PaymentDescription { get; set; } = Constants.DefaultValues.PaymentDescription;//  (if Status is Rejected)
    public virtual User User { get; set; }
    public virtual ExpenseCategory ExpenseCategory { get; set; }
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
        
        builder.Property(e => e.PaymentStatus).IsRequired().HasConversion<string>();

        builder.HasOne(e => e.User)
            .WithMany(u => u.ExpenseRequests)
            .HasForeignKey(e => e.UserId);

        builder.HasOne(e => e.ExpenseCategory)
            .WithMany()
            .HasForeignKey(e => e.CategoryId);
            

        
    }
}