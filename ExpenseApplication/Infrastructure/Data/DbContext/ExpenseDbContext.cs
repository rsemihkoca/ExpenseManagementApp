using Business.Entities;
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
    }
}