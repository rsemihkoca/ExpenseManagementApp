using Business.Entities;
using Business.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.DbContext;

public class ExpenseDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options) : base(options)
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

        modelBuilder.Entity<ExpenseCategory>().HasData(
            new ExpenseCategory // Reklam
            {
                CategoryId = 1,
                CategoryName = "Advertising"
            },
            new ExpenseCategory // Elektrik, Su, Fatura
            {
                CategoryId = 2,
                CategoryName = "Utilities"
            },
            new ExpenseCategory // Maaş ve İşçi Giderleri
            {
                CategoryId = 3,
                CategoryName = "Payroll"
            },
            new ExpenseCategory // Sigorta
            {
                CategoryId = 4,
                CategoryName = "Insurance"
            },
            new ExpenseCategory // Kira
            {
                CategoryId = 5,
                CategoryName = "Rent"
            },
            new ExpenseCategory // Seyahat Giderleri
            {
                CategoryId = 6,
                CategoryName = "Travel Expenses"
            },
            new ExpenseCategory // Bakım ve Onarım
            {
                CategoryId = 7,
                CategoryName = "Maintenance and Repairs"
            },
            new ExpenseCategory // Üyelik Ücretleri
            {
                CategoryId = 8,
                CategoryName = "Membership Fees"
            },
            new ExpenseCategory // İş Lisansları ve İzinler
            {
                CategoryId = 9,
                CategoryName = "Business Licenses and Permits"
            },
            new ExpenseCategory // Eğitim
            {
                CategoryId = 10,
                CategoryName = "Education"
            },
            new ExpenseCategory // Sabit Giderler
            {
                CategoryId = 11,
                CategoryName = "Fixed Expenses"
            },
            new ExpenseCategory // Yiyecek
            {
                CategoryId = 12,
                CategoryName = "Food"
            },
            new ExpenseCategory // Eğlence
            {
                CategoryId = 13,
                CategoryName = "Entertainment"
            },
            new ExpenseCategory // Vergiler
            {
                CategoryId = 14,
                CategoryName = "Taxes"
            },
            new ExpenseCategory // Hediyeler
            {
                CategoryId = 15,
                CategoryName = "Gifts"
            },
            new ExpenseCategory // Ofis Ekipmanları
            {
                CategoryId = 16,
                CategoryName = "Office Equipment"
            }
        );


        modelBuilder.Entity<PaymentInstruction>().HasData(
            new PaymentInstruction()
            {
                PaymentInstructionId = 1,
                ExpenseRequestId = 1,
                PaymentStatus = PaymentRequestStatus.Completed,
                PaymentDate = DateTime.Parse("2023-05-13T12:30:00")
            }
        );

        modelBuilder.Entity<Expense>().HasData(
            new Expense
            {
                ExpenseRequestId = 1,
                UserId = 1,
                Amount = 1500,
                CategoryId = 1,
                PaymentMethod = "Credit Card",
                PaymentLocation = "Online",
                Documents = "Receipt123",
                Status = ExpenseRequestStatus.Approved,
                Description = "Reklam için ödeme yapıldı.",
                CreationDate = DateTime.Parse("2023-05-12T08:30:00"),
                LastUpdateTime = DateTime.Parse("2023-05-13T12:30:00")
            },
            new Expense
            {
                ExpenseRequestId = 2,
                UserId = 2,
                Amount = 350,
                CategoryId = 2,
                PaymentMethod = "Bank Transfer",
                PaymentLocation = "Office",
                Documents = "Invoice456",
                Status = ExpenseRequestStatus.Pending,
                Description = "Elektrik faturası ödendi.",
                CreationDate = DateTime.Parse("2023-08-22T15:45:00"),
                LastUpdateTime = DateTime.Parse("2023-08-22T15:45:00")
            },
            new Expense
            {
                ExpenseRequestId = 3,
                UserId = 3,
                Amount = 2500,
                CategoryId = 3,
                PaymentMethod = "Cash",
                PaymentLocation = "Office",
                Documents = "PayrollDocs",
                Status = ExpenseRequestStatus.Pending,
                Description = "Personel maaşları ödendi.",
                CreationDate = DateTime.Parse("2023-06-10T12:15:00"),
                LastUpdateTime = DateTime.Parse("2023-06-10T12:15:00")
            },
            new Expense
            {
                ExpenseRequestId = 4,
                UserId = 4,
                Amount = 700,
                CategoryId = 4,
                PaymentMethod = "Credit Card",
                PaymentLocation = "Online",
                Documents = "InsurancePolicy",
                Status = ExpenseRequestStatus.Pending,
                Description = "Sigorta ödemesi yapıldı.",
                CreationDate = DateTime.Parse("2023-09-05T09:20:00"),
                LastUpdateTime = DateTime.Parse("2023-09-05T09:20:00")
            },
            new Expense
            {
                ExpenseRequestId = 5,
                UserId = 1,
                Amount = 1200,
                CategoryId = 5,
                PaymentMethod = "Bank Transfer",
                PaymentLocation = "Office",
                Documents = "RentAgreement",
                Status = ExpenseRequestStatus.Pending,
                Description = "Kira ödemesi yapıldı.",
                CreationDate = DateTime.Parse("2023-11-18T14:00:00"),
                LastUpdateTime = DateTime.Parse("2023-11-18T14:00:00")
            },
            new Expense
            {
                ExpenseRequestId = 6,
                UserId = 2,
                Amount = 450,
                CategoryId = 6,
                PaymentMethod = "Cash",
                PaymentLocation = "Travel",
                Documents = "TravelReceipts",
                Status = ExpenseRequestStatus.Pending,
                Description = "Seyahat giderleri ödendi.",
                CreationDate = DateTime.Parse("2023-07-03T11:30:00"),
                LastUpdateTime = DateTime.Parse("2023-07-03T11:30:00")
            },
            new Expense
            {
                ExpenseRequestId = 7,
                UserId = 3,
                Amount = 300,
                CategoryId = 7,
                PaymentMethod = "Credit Card",
                PaymentLocation = "Maintenance",
                Documents = "RepairReceipts",
                Status = ExpenseRequestStatus.Pending,
                Description = "Bakım ve onarım masrafları ödendi.",
                CreationDate = DateTime.Parse("2023-10-29T16:45:00"),
                LastUpdateTime = DateTime.Parse("2023-10-29T16:45:00")
            },
            new Expense
            {
                ExpenseRequestId = 8,
                UserId = 4,
                Amount = 180,
                CategoryId = 8,
                PaymentMethod = "Bank Transfer",
                PaymentLocation = "Online",
                Documents = "MembershipFeesReceipts",
                Status = ExpenseRequestStatus.Pending,
                Description = "Üyelik ücretleri ödendi.",
                CreationDate = DateTime.Parse("2023-12-08T10:10:00"),
                LastUpdateTime = DateTime.Parse("2023-12-08T10:10:00")
            },
            new Expense
            {
                ExpenseRequestId = 9,
                UserId = 1,
                Amount = 500,
                CategoryId = 9,
                PaymentMethod = "Cash",
                PaymentLocation = "Office",
                Documents = "LicensesAndPermitsDocs",
                Status = ExpenseRequestStatus.Pending,
                Description = "İş lisansları ve izinler ödendi.",
                CreationDate = DateTime.Parse("2023-08-14T13:20:00"),
                LastUpdateTime = DateTime.Parse("2023-08-14T13:20:00")
            },
            new Expense
            {
                ExpenseRequestId = 10,
                UserId = 2,
                Amount = 800,
                CategoryId = 10,
                PaymentMethod = "Credit Card",
                PaymentLocation = "Online",
                Documents = "EducationReceipts",
                Status = ExpenseRequestStatus.Pending,
                Description = "Eğitim masrafları ödendi.",
                CreationDate = DateTime.Parse("2023-05-26T09:55:00"),
                LastUpdateTime = DateTime.Parse("2023-05-26T09:55:00")
            },
            new Expense
            {
                ExpenseRequestId = 11,
                UserId = 3,
                Amount = 150,
                CategoryId = 11,
                PaymentMethod = "Bank Transfer",
                PaymentLocation = "Office",
                Documents = "FixedExpensesReceipts",
                Status = ExpenseRequestStatus.Pending,
                Description = "Sabit giderler ödendi.",
                CreationDate = DateTime.Parse("2023-09-18T11:40:00"),
                LastUpdateTime = DateTime.Parse("2023-09-18T11:40:00")
            },
            new Expense
            {
                ExpenseRequestId = 12,
                UserId = 4,
                Amount = 75,
                CategoryId = 12,
                PaymentMethod = "Cash",
                PaymentLocation = "Office",
                Documents = "FoodReceipts",
                Status = ExpenseRequestStatus.Pending,
                Description = "Yiyecek alımı yapıldı.",
                CreationDate = DateTime.Parse("2023-07-22T14:30:00"),
                LastUpdateTime = DateTime.Parse("2023-07-22T14:30:00")
            },
            new Expense
            {
                ExpenseRequestId = 13,
                UserId = 1,
                Amount = 200,
                CategoryId = 13,
                PaymentMethod = "Credit Card",
                PaymentLocation = "Online",
                Documents = "EntertainmentReceipts",
                Status = ExpenseRequestStatus.Pending,
                Description = "Eğlence masrafları ödendi.",
                CreationDate = DateTime.Parse("2023-11-03T15:15:00"),
                LastUpdateTime = DateTime.Parse("2023-11-03T15:15:00")
            },
            new Expense
            {
                ExpenseRequestId = 14,
                UserId = 2,
                Amount = 300,
                CategoryId = 14,
                PaymentMethod = "Bank Transfer",
                PaymentLocation = "Office",
                Documents = "TaxDocuments",
                Status = ExpenseRequestStatus.Pending,
                Description = "Vergi ödemeleri yapıldı.",
                CreationDate = DateTime.Parse("2023-06-28T10:50:00"),
                LastUpdateTime = DateTime.Parse("2023-06-28T10:50:00")
            },
            new Expense
            {
                ExpenseRequestId = 15,
                UserId = 3,
                Amount = 50,
                CategoryId = 15,
                PaymentMethod = "Cash",
                PaymentLocation = "Office",
                Documents = "GiftReceipts",
                Status = ExpenseRequestStatus.Pending,
                Description = "Hediye alımları yapıldı.",
                CreationDate = DateTime.Parse("2023-10-11T12:05:00"),
                LastUpdateTime = DateTime.Parse("2023-10-11T12:05:00")
            },
            new Expense
            {
                ExpenseRequestId = 16,
                UserId = 4,
                Amount = 1000,
                CategoryId = 16,
                PaymentMethod = "Credit Card",
                PaymentLocation = "Online",
                Documents = "OfficeEquipmentReceipts",
                Status = ExpenseRequestStatus.Pending,
                Description = "Ofis ekipmanları alındı.",
                CreationDate = DateTime.Parse("2023-12-15T09:00:00"),
                LastUpdateTime = DateTime.Parse("2023-12-15T09:00:00")
            },
            new Expense
            {
                ExpenseRequestId = 17,
                UserId = 1,
                Amount = 400,
                CategoryId = 3,
                PaymentMethod = "Bank Transfer",
                PaymentLocation = "Office",
                Documents = "PayrollDocs",
                Status = ExpenseRequestStatus.Pending,
                Description = "Personel maaşları ödendi.",
                CreationDate = DateTime.Parse("2023-09-01T14:30:00"),
                LastUpdateTime = DateTime.Parse("2023-09-01T14:30:00")
            },
            new Expense
            {
                ExpenseRequestId = 18,
                UserId = 2,
                Amount = 600,
                CategoryId = 5,
                PaymentMethod = "Cash",
                PaymentLocation = "Office",
                Documents = "RentAgreement",
                Status = ExpenseRequestStatus.Pending,
                Description = "Kira ödemesi yapıldı.",
                CreationDate = DateTime.Parse("2023-07-14T16:20:00"),
                LastUpdateTime = DateTime.Parse("2023-07-14T16:20:00")
            },
            new Expense
            {
                ExpenseRequestId = 19,
                UserId = 3,
                Amount = 250,
                CategoryId = 6,
                PaymentMethod = "Credit Card",
                PaymentLocation = "Travel",
                Documents = "TravelReceipts",
                Status = ExpenseRequestStatus.Pending,
                Description = "Seyahat giderleri ödendi.",
                CreationDate = DateTime.Parse("2023-10-05T10:45:00"),
                LastUpdateTime = DateTime.Parse("2023-10-05T10:45:00")
            },
            new Expense
            {
                ExpenseRequestId = 20,
                UserId = 4,
                Amount = 120,
                CategoryId = 8,
                PaymentMethod = "Bank Transfer",
                PaymentLocation = "Online",
                Documents = "MembershipFeesReceipts",
                Status = ExpenseRequestStatus.Pending,
                Description = "Üyelik ücretleri ödendi.",
                CreationDate = DateTime.Parse("2023-11-29T09:10:00"),
                LastUpdateTime = DateTime.Parse("2023-11-29T09:10:00")
            },
            new Expense
            {
                ExpenseRequestId = 21,
                UserId = 1,
                Amount = 300,
                CategoryId = 10,
                PaymentMethod = "Cash",
                PaymentLocation = "Office",
                Documents = "EducationReceipts",
                Status = ExpenseRequestStatus.Pending,
                Description = "Eğitim masrafları ödendi.",
                CreationDate = DateTime.Parse("2023-08-08T12:35:00"),
                LastUpdateTime = DateTime.Parse("2023-08-08T12:35:00")
            }
        );
    }
}

//
// -- Dummy data for Expense table
// INSERT INTO db.CaseDb.Expense
// (UserId, Amount, CategoryId, PaymentMethod, PaymentLocation, Documents, Status, Description, CreationDate, LastUpdateTime)
// VALUES
// (1, 1500, 1, 'Credit Card', 'Online', 'Receipt123', 1, 'Reklam için ödeme yapıldı.', '2023-05-12T08:30:00', '2023-05-12T08:30:00'),
//
// (2, 350, 2, 'Bank Transfer', 'Office', 'Invoice456', 1, 'Elektrik faturası ödendi.', '2023-08-22T15:45:00', '2023-08-22T15:45:00'),
//
// (3, 2500, 3, 'Cash', 'Office', 'PayrollDocs', 1, 'Personel maaşları ödendi.', '2023-06-10T12:15:00', '2023-06-10T12:15:00'),
//
// (4, 700, 4, 'Credit Card', 'Online', 'InsurancePolicy', 1, 'Sigorta ödemesi yapıldı.', '2023-09-05T09:20:00', '2023-09-05T09:20:00'),
//
// (1, 1200, 5, 'Bank Transfer', 'Office', 'RentAgreement', 1, 'Kira ödemesi yapıldı.', '2023-11-18T14:00:00', '2023-11-18T14:00:00'),
//
// (2, 450, 6, 'Cash', 'Travel', 'TravelReceipts', 1, 'Seyahat giderleri ödendi.', '2023-07-03T11:30:00', '2023-07-03T11:30:00'),
//
// (3, 300, 7, 'Credit Card', 'Maintenance', 'RepairReceipts', 1, 'Bakım ve onarım masrafları ödendi.', '2023-10-29T16:45:00', '2023-10-29T16:45:00'),
//
// (4, 180, 8, 'Bank Transfer', 'Online', 'MembershipFeesReceipts', 1, 'Üyelik ücretleri ödendi.', '2023-12-08T10:10:00', '2023-12-08T10:10:00'),
//
// (1, 500, 9, 'Cash', 'Office', 'LicensesAndPermitsDocs', 1, 'İş lisansları ve izinler ödendi.', '2023-08-14T13:20:00', '2023-08-14T13:20:00'),
//
// (2, 800, 10, 'Credit Card', 'Online', 'EducationReceipts', 1, 'Eğitim masrafları ödendi.', '2023-05-26T09:55:00', '2023-05-26T09:55:00'),
//
// (3, 150, 11, 'Bank Transfer', 'Office', 'FixedExpensesReceipts', 1, 'Sabit giderler ödendi.', '2023-09-18T11:40:00', '2023-09-18T11:40:00'),
//
// (4, 75, 12, 'Cash', 'Office', 'FoodReceipts', 1, 'Yiyecek alımı yapıldı.', '2023-07-22T14:30:00', '2023-07-22T14:30:00'),
//
// (1, 200, 13, 'Credit Card', 'Online', 'EntertainmentReceipts', 1, 'Eğlence masrafları ödendi.', '2023-11-03T15:15:00', '2023-11-03T15:15:00'),
//
// (2, 300, 14, 'Bank Transfer', 'Office', 'TaxDocuments', 1, 'Vergi ödemeleri yapıldı.', '2023-06-28T10:50:00', '2023-06-28T10:50:00'),
//
// (3, 50, 15, 'Cash', 'Office', 'GiftReceipts', 1, 'Hediye alımları yapıldı.', '2023-10-11T12:05:00', '2023-10-11T12:05:00'),
//
// (4, 1000, 16, 'Credit Card', 'Online', 'OfficeEquipmentReceipts', 1, 'Ofis ekipmanları alındı.', '2023-12-15T09:00:00', '2023-12-15T09:00:00'),
//
// (1, 400, 3, 'Bank Transfer', 'Office', 'PayrollDocs', 1, 'Personel maaşları ödendi.', '2023-09-01T14:30:00', '2023-09-01T14:30:00'),
//
// (2, 600, 5, 'Cash', 'Office', 'RentAgreement', 1, 'Kira ödemesi yapıldı.', '2023-07-14T16:20:00', '2023-07-14T16:20:00'),
//
// (3, 250, 6, 'Credit Card', 'Travel', 'TravelReceipts', 1, 'Seyahat giderleri ödendi.', '2023-10-05T10:45:00', '2023-10-05T10:45:00'),
//
// (4, 120, 8, 'Bank Transfer', 'Online', 'MembershipFeesReceipts', 1, 'Üyelik ücretleri ödendi.', '2023-11-29T09:10:00', '2023-11-29T09:10:00'),
//
// (1, 300, 10, 'Cash', 'Office', 'EducationReceipts', 1, 'Eğitim masrafları ödendi.', '2023-08-08T12:35:00', '2023-08-08T12:35:00');