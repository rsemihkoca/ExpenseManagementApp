using Business.Cqrs;
using Infrastructure.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Schemes.Dtos;

namespace Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly IMediator mediator;

    public TokenController(IMediator mediator)
    {
        this.mediator = mediator;
    }
    
            
    [HttpPost]
    public async Task<TokenResponse> Post([FromBody] TokenRequest request)
    {
        var operation = new CreateTokenCommand(request);
        var result = await mediator.Send(operation);
        return result;
    }
}