namespace Infrastructure.Dtos;

public class TokenRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class TokenResponse
{
    public DateTime ExpireDate { get; set; }
    public string Token { get; set; }
    public string Email { get; set; }
}