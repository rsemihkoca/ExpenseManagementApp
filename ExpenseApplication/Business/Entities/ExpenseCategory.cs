using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Business.Entities;

[Table("ExpenseCategory", Schema = "dbo")]
public class ExpenseCategory : BaseEntity
{
    public int CategoryId { get; set; } // (Primary Key)
    public string CategoryName { get; set; }

    public virtual ICollection<Expense> ExpenseRequests { get; set; }
}

public class ExpenseCategoryConfiguration : IEntityTypeConfiguration<ExpenseCategory>
{
    public void Configure(EntityTypeBuilder<ExpenseCategory> builder)
    {
        builder.HasKey(e => e.CategoryId);
        builder.HasIndex(e => e.CategoryId).IsUnique();
        
        builder.Property(e => e.CategoryName).IsRequired().HasMaxLength(255);
        
        // Additional configuration for expense category entity
        builder.HasMany(e => e.ExpenseRequests)
            .WithOne(e => e.ExpenseCategory)
            .HasForeignKey(e => e.CategoryId);
            // .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of ExpenseCategory if it is referenced by an Expense
    }
}