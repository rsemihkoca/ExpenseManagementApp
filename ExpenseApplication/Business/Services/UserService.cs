using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Schemes.Exceptions;

namespace Business.Services;


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
            throw new HttpException(Constants.ErrorMessages.RoleNotFound, 404);
        }

        return role;
    }

    public string GetUserEmail()
    {
        var email = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
        
        if (email == null)
        {
            throw new HttpException(Constants.ErrorMessages.EmailNotFound, 404);
        }

        return email;
    }
    
    public int GetUserId()
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirst(Constants.Credentials.Id)?.Value;
        
        if (userId == null)
        {
            throw new HttpException(Constants.ErrorMessages.UserNotFound, 404);
        }
        return int.Parse(userId);
    }
}