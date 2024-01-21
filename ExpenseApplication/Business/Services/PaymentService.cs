namespace Application.Services;

using System;
using System.Net.Http;
using System.Threading.Tasks;

public interface IPaymentService
{
    Task ProcessPayment(decimal amount, string fromIban, string toIban);
    
}

public class PaymentService : IPaymentService
{
    public async Task ProcessPayment(decimal amount, string fromIban, string toIban)
    {

        bool paymentSuccess = await MakePaymentRequest(amount, toIban);

        if (paymentSuccess)
        {
            // Update Status of Payment Instruction to Success
            // send Email to toEmail
        }
        else
        {
        }
    }

    private async Task<bool> MakePaymentRequest(decimal amount, string toIban)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                string apiUrl = "http://localhost:5245/api/PaymentSimulator/ProcessPayment";
                
                // Request to PaymentSimulator API
                HttpResponseMessage response = await client.PostAsync(apiUrl, new StringContent($"{{\"amount\": {amount}, \"iban\": \"{toIban}\"}}", System.Text.Encoding.UTF8, "application/json"));

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
        catch (Exception ex)
        {
            Console.WriteLine($"Error during payment request: {ex.Message}");
            return false;
        }
    }
    
    
}