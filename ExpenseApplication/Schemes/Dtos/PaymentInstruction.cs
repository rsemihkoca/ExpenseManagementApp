namespace Infrastructure.Dtos;

public class CreateUserRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Iban { get; set; }
    public string Role { get; set; }
}

public class UpdateUserRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Iban { get; set; }
    public string Role { get; set; }
    public bool IsActive { get; set; }
}

public class UserResponse
{
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Iban { get; set; }
    public string Role { get; set; }
    public string IsActive { get; set; }
    public string LastActivityDateTime { get; set; }
}