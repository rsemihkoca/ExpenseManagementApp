using Microsoft.AspNetCore.Mvc;


[Route("api/[controller]")]
[ApiController]
public class PaymentSimulatorController : ControllerBase
{
    private readonly Random random = new Random();

    [HttpPost("ProcessPayment")]
    public IActionResult ProcessPayment([FromBody] PaymentRequest request)
    {
        Thread.Sleep(5000);

        if (IsValidPayment(request))
        {
            double randomNumber = random.NextDouble();

            if (randomNumber < 0.6)
            {
                // 60% chance for a successful payment
                var response = new PaymentResponse
                {
                    Status = "success",
                    Message = "Payment successful. From IBAN: " + request.FromIBAN + ", To IBAN: " + request.ToIBAN + ", Amount: " + request.Amount,
                };

                return Ok(response);
            }
            else if (randomNumber < 0.8 && randomNumber >= 0.6)
            {
                // 20% chance for an error with invalid payment details
                var response = new PaymentResponse
                {
                    Status = "error",
                    Message = "Invalid payment details. Please check your information and try again.",
                };

                return BadRequest(response);
            }
            else
            {
                // 20% chance for an error with an unspecified reason
                var response = new PaymentResponse
                {
                    Status = "error",
                    Message = "An error occurred while processing the payment. Please try again later.",
                };

                return BadRequest(response);
            }
        }
        else
        {
            // Payment failed response for invalid payment details
            var response = new PaymentResponse
            {
                Status = "error",
                Message = "Invalid payment details. Please check your information and try again.",
            };

            return BadRequest(response);
        }
    }

    private bool IsValidPayment(PaymentRequest request)
    {
        if (request.Amount <= 0)
        {
            return false;
        }
        if (string.IsNullOrEmpty(request.FromIBAN))
        {
            return false;
        }
        if (string.IsNullOrEmpty(request.ToIBAN))
        {
            return false;
        }
        return true;
    }
}

public class PaymentRequest
{
    public decimal Amount { get; set; }
    public string FromIBAN { get; set; }
    public string ToIBAN { get; set; }
}

public class PaymentResponse
{
    public string Status { get; set; }
    public string Message { get; set; }
}
