using System.ComponentModel.DataAnnotations.Schema;
using Business.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Business.Entities;

[Table("Users", Schema = "CaseDb")]
public class User : BaseEntity
{
    public int UserId { get; set; } // Primary key
    public string Username { get; set; }
    public string Password { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Iban { get; set; }
    public UserRole Role { get; set; } // (Admin or Personnel)
    public int PasswordRetryCount { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime LastActivityDateTime { get; set; }

    public virtual ICollection<Expense> ExpenseRequests { get; set; }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.UserId);
        builder.HasIndex(u => u.UserId).IsUnique();
        
        builder.Property(u => u.Username).IsRequired().HasMaxLength(50);
        builder.HasIndex(u => u.Username).IsUnique();

        builder.Property(u => u.Password).IsRequired().HasMaxLength(255);

        builder.Property(u => u.FirstName).IsRequired().HasMaxLength(50);

        builder.Property(u => u.LastName).IsRequired().HasMaxLength(50);

        builder.Property(u => u.Email).IsRequired().HasMaxLength(255);
        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.Iban).IsRequired().HasMaxLength(34);
        builder.HasIndex(u => u.Iban).IsUnique();

        builder.Property(u => u.Role).IsRequired();

        builder.Property(u => u.PasswordRetryCount).IsRequired();

        builder.Property(u => u.IsActive).IsRequired();
        
        builder.Property(u => u.LastActivityDateTime).IsRequired();
        
        
        builder.HasMany(u => u.ExpenseRequests)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId);
        // .OnDelete(DeleteBehavior.Restrict);


    }
}