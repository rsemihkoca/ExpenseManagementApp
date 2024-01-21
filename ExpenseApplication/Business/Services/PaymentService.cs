using System.Net.Http.Json;
using Business.Entities;
using Infrastructure.Data.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

using System;
using System.Net.Http;
using System.Threading.Tasks;

public interface IPaymentService
{
    Task ProcessPayment(decimal amount, string fromIban, string toIban);
    // Get Payment Information
    
    Task<(string?, string?)> GetPaymentCredentials(int fromUserId, int toUserId);
}

public class PaymentService : IPaymentService
{
    private readonly ExpenseDbContext dbContext;
    
    public async Task ProcessPayment(decimal amount, string fromUserId, string toUserId)
    {
        
        (string? fromIban, string? toIban) = await GetPaymentCredentials(int.Parse(fromUserId), int.Parse(toUserId));
        
        if (fromIban == null || toIban == null)
        {
            throw new Exception("Payment Credentials Not Found");
        }
        
        bool paymentSuccess = await MakePaymentRequest(amount, fromIban, toIban);

        if (paymentSuccess)
        {
            // Update Status of Payment Instruction to Success
            // send Email to toEmail
            Console.WriteLine("Payment Success");
        }
        else
        {
            // Update Status of Payment Instruction to Failed
            // send Email to toEmail
            Console.WriteLine("Payment Failed");
        }
    }

    public async Task<(string?, string?)> GetPaymentCredentials(int fromUserId, int toUserId)
    {
        var fromUser = await dbContext.Set<User>().FirstOrDefaultAsync(x => x.UserId == fromUserId);
        var toUser = await dbContext.Set<User>().FirstOrDefaultAsync(x => x.UserId == toUserId);
        
        return fromUser != null && toUser != null ? (fromUser.Iban, toUser.Iban) : (null, null);
    }

    private async Task<bool> MakePaymentRequest(decimal amount, string fromIban, string toIban)
    {
        var client = new HttpClient();
        var request = new PaymentRequest()
        {
            Amount = amount,
            FromIBAN = fromIban,
            ToIBAN = toIban
        };
        var response = await client.PostAsJsonAsync("https://localhost:5001/api/payment", request);
        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}

public class PaymentRequest
{
    public decimal Amount { get; set; }
    public string FromIBAN { get; set; }
    public string ToIBAN { get; set; }
}