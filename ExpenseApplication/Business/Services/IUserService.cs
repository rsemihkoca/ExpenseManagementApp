namespace Application.Services;

public interface IUserService
{
    string? GetUserRole();
    
    string? GetUserEmail();
    
    int GetUserId();
    
}