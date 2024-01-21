using System.Security.Claims;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Application.Services;


public interface IUserService
{
    string? GetUserRole();
    
    string? GetUserEmail();
    
    int GetUserId();
    
}


public class UserService(IHttpContextAccessor httpContextAccessor) : IUserService
{
    public string GetUserRole()
    {
        var role = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
        
        if (role == null)
        {
            throw new HttpException("Role not found", 404);
        }

        return role;
    }

    public string GetUserEmail()
    {
        var email = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
        
        if (email == null)
        {
            throw new HttpException("Email not found", 404);
        }

        return email;
    }
    
    public int GetUserId()
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirst("Id")?.Value;
        
        if (userId == null)
        {
            throw new HttpException("User not found", 404);
        }
        return int.Parse(userId);
    }
}