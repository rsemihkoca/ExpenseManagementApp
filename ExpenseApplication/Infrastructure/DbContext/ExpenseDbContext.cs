using Business.Entities;
using Business.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.DbContext;

public class ExpenseDbContext : Microsoft.EntityFrameworkCore.DbContext
{

    public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options): base(options)
    {
    
    }   
    public DbSet<User> Users { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
    public DbSet<PaymentInstruction> PaymentInstructions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new ExpenseRequestConfiguration());
        modelBuilder.ApplyConfiguration(new ExpenseCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentInstructionConfiguration());
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>().HasData(
        
        
            new User
            {
                UserId = 1,
                Username = "admin1",
                Password = "5F4DCC3B5AA765D61D8327DEB882CF99",
                FirstName = "Admin",
                LastName = "1",
                Email = "admin1@example.com",
                Iban = "TR760009901234567800100001",
                Role = UserRole.Admin,
                PasswordRetryCount = 0,
                IsActive = true,
                LastActivityDateTime = DateTime.Now
            },
            new User
            {
                UserId = 2,
                Username = "admin2",
                Password = "5F4DCC3B5AA765D61D8327DEB882CF99", // "password"
                FirstName = "Admin",
                LastName = "2",
                Email = "admin2@example.com",
                Iban = "TR548695845712502365201452",
                Role = UserRole.Admin,
                PasswordRetryCount = 0,
                IsActive = true,
                LastActivityDateTime = DateTime.Now
            },
            new User
            {
                UserId = 3,
                Username = "personnel1",
                Password = "5F4DCC3B5AA765D61D8327DEB882CF99",
                FirstName = "Personnel",
                LastName = "1",
                Email = "personnel1@example.com",
                Iban = "TR115181625282523330364444",
                Role = UserRole.Personnel,
                PasswordRetryCount = 0,
                IsActive = true,
                LastActivityDateTime = DateTime.Now
            },
            new User
            {
                UserId = 4,
                Username = "personnel2",
                Password = "5F4DCC3B5AA765D61D8327DEB882CF99",
                FirstName = "Personnel",
                LastName = "2",
                Email = "personnel2@example.com",
                Iban = "TR960251857420045115789005",
                Role = UserRole.Personnel,
                PasswordRetryCount = 0,
                IsActive = true,
                LastActivityDateTime = DateTime.Now
            }
        );
    }
}