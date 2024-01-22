using System.Net.Http.Json;
using Infrastructure.DbContext;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Schemes.Enums;

namespace Business.Services;

using System;
using System.Net.Http;
using System.Threading.Tasks;

public interface IPaymentService
{
    // Task<ExecuteProcessResponse> ExecutePayment(double amount, int fromIban, int toIban);

    Task ProcessPayment(int expenseRequestId, double amount, int fromUserId, int toUserId);
    // Task<(User? fromUser, User? toUser)> GetPaymentCredentials(int fromUserId, int toUserId);
}

public class PaymentService : IPaymentService
{
    private readonly ExpenseDbContext dbContext;

    public PaymentService(ExpenseDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task ProcessPayment(int expenseRequestId, double amount, int fromUserId, int toUserId)
    {
        ExecuteProcessResponse response = await ExecutePayment(amount, fromUserId, toUserId);
        var expense = await dbContext.Set<Expense>().FirstOrDefaultAsync(x => x.ExpenseRequestId == expenseRequestId);

        switch (response.paymentSuccess)
        {
            case true:
                // get expense from db and update status
                // var expense = await dbContext.Set<Expense>().FirstOrDefaultAsync(x => x.ExpenseRequestId == credentials.fromUser.);

                Console.WriteLine("Payment Success : " + response.message);
                
                expense.LastUpdateTime = DateTime.Now;
                expense.PaymentStatus = PaymentRequestStatus.Completed;
                expense.PaymentDescription = response.message;
                expense.PaymentDate = DateTime.Now;
                
                await dbContext.SaveChangesAsync();
                
                Console.WriteLine($"Email sent to {response.fromUserEmail}: ${amount} Paid Successfully");
                break;
            case false:

                Console.WriteLine("Payment Failed : " + response.message);
                
                expense.LastUpdateTime = DateTime.Now;
                expense.PaymentStatus = PaymentRequestStatus.Failed;
                expense.PaymentDescription = response.message;
                expense.PaymentDate = DateTime.Now;
                
                await dbContext.SaveChangesAsync();

                Console.WriteLine($"Email sent to {response.fromUserEmail} Failed");
                break;
        }
    }

    private async Task<ExecuteProcessResponse> ExecutePayment(double amount, int fromUserId, int toUserId)
    {
        (User? fromUser, User? toUser) credentials = await GetPaymentCredentials(fromUserId, toUserId);

        if (credentials.fromUser?.Iban == null || credentials.toUser?.Iban == null)
        {
            throw new Exception("Payment Credentials Not Found");
        }

        (bool paymentSuccess, string message) response =
            await MakePaymentRequest(amount, credentials.fromUser.Iban, credentials.toUser.Iban);

        return new ExecuteProcessResponse()
        {
            paymentSuccess = response.paymentSuccess,
            message = response.message,
            fromUserEmail = credentials.fromUser.Email,
            toUserEmail = credentials.toUser.Email
        };
    }

    private async Task<(User? fromUser, User? toUser)> GetPaymentCredentials(int fromUserId, int toUserId)
    {
        var fromUser = await dbContext.Set<User>().FirstOrDefaultAsync(x => x.UserId == fromUserId);
        var toUser = await dbContext.Set<User>().FirstOrDefaultAsync(x => x.UserId == toUserId);

        return fromUser != null && toUser != null ? (fromUser, toUser) : (null, null);
    }

    private async Task<(bool, string)> MakePaymentRequest(double amount, string fromIban, string toIban)
    {
        var client = new HttpClient();
        var request = new PaymentRequest()
        {
            Amount = amount,
            FromIBAN = fromIban,
            ToIBAN = toIban
        };
        var response =
            await client.PostAsJsonAsync("http://localhost:5245/api/PaymentSimulator/ProcessPayment", request);

        var result = await response.Content.ReadFromJsonAsync<PaymentResponse>();
        if (response.IsSuccessStatusCode)
        {
            return (true, result.Message);
        }
        else
        {
            return (false, result.Message);
        }
    }
}

public class PaymentRequest
{
    public double Amount { get; set; }
    public string FromIBAN { get; set; }
    public string ToIBAN { get; set; }
}

public class PaymentResponse
{
    public string Status { get; set; }
    public string Message { get; set; }
}

public class ExecuteProcessResponse
{
    public bool paymentSuccess { get; set; }
    public string message { get; set; }
    public string fromUserEmail { get; set; }
    public string toUserEmail { get; set; }
}