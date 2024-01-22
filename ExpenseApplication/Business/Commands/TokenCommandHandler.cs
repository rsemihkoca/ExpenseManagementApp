using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Business.Cqrs;
using Infrastructure.DbContext;
using Infrastructure.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Schemes.Dtos;
using Schemes.Exceptions;
using Schemes.Token;

namespace Business.Commands;

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
            throw new HttpException(Constants.ErrorMessages.InvalidUserInformation, 400);
        }

        if (!user.IsActive)
        {
            throw new HttpException(Constants.ErrorMessages.ContactAdministrator, 403);
        }

        string hash = MD5Extensions.ToMD5(request.Model.Password.Trim());
        if (hash != user.Password)
        {
            user.LastActivityDateTime = DateTime.UtcNow;
            user.PasswordRetryCount++;
            await dbContext.SaveChangesAsync(cancellationToken);
            throw new HttpException($"Credentials are incorrect. You have {3 - user.PasswordRetryCount} attempts left.", 401);
        }

        if (user.PasswordRetryCount > 3)
        {
            throw new HttpException(Constants.ErrorMessages.ContactAdministrator, 405);
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
            new Claim(Constants.Credentials.Id, user.UserId.ToString()),
            new Claim(Constants.Credentials.Email, user.Email),
            new Claim(Constants.Credentials.Username, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
        };

        return claims;
    }
}