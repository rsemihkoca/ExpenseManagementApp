using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Cqrs;
using Business.Entities;
using Infrastructure.Data.DbContext;
using Infrastructure.Dtos;
using Infrastructure.Token;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Commands;

public class TokenCommandHandler :
    IRequestHandler<CreateTokenCommand, TokenResponse>
{
    private readonly ExpenseDbContext dbContext;
    private readonly JwtConfig jwtConfig;

    public TokenCommandHandler(ExpenseDbContext dbContext, IOptionsMonitor<JwtConfig> jwtConfig)
    {
        this.dbContext = dbContext;
        this.jwtConfig = jwtConfig.CurrentValue;
    }

    public async Task<TokenResponse> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Set<User>().Where(x => x.Username == request.Model.UserName)
            .FirstOrDefaultAsync(cancellationToken);
            
        if (user == null)
        {
            throw new Exception("Invalid user information");
        }

        if (!user.IsActive)
        {
            throw new Exception("Please contact your administrator, your account is locked.");
        }

        string hash = MD5Extensions.ToMD5(request.Model.Password.Trim());
        if (hash != user.Password)
        {
            user.LastActivityDateTime = DateTime.UtcNow;
            user.PasswordRetryCount++;
            await dbContext.SaveChangesAsync(cancellationToken);
            throw new Exception($"Password is incorrect. You have {3 - user.PasswordRetryCount} attempts left.");
        }

        if (user.PasswordRetryCount > 3)
        {
            throw new Exception("Please contact your administrator, your account is locked.");
        }

        user.LastActivityDateTime = DateTime.UtcNow;
        user.PasswordRetryCount = 0;
        await dbContext.SaveChangesAsync(cancellationToken);

        string token = Token(user);

        return new TokenResponse()
        {
            Email = user.Email,
            Token = token,
            ExpireDate = DateTime.Now.AddMinutes(jwtConfig.AccessTokenExpiration)
        };
    }

    private string Token(User user)
    {
        Claim[] claims = GetClaims(user);
        var secret = Encoding.ASCII.GetBytes(jwtConfig.Secret);

        var jwtToken = new JwtSecurityToken(
            jwtConfig.Issuer,
            jwtConfig.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(jwtConfig.AccessTokenExpiration),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secret),
                SecurityAlgorithms.HmacSha256Signature)
        );

        string accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        return accessToken;
    }

    private Claim[] GetClaims(User user)
    {
        var claims = new[]
        {
            new Claim("Id", user.UserId.ToString()),
            new Claim("Email", user.Email),
            new Claim("Username", user.Username),
            new Claim("Role", user.Role.ToString()),
        };

        return claims;
    }
}