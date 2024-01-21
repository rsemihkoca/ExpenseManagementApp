using Application.Cqrs;
using Infrastructure.Dtos;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var command = new CreateUserCommand(request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{UserId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUser(int UserId, [FromBody] UpdateUserRequest request)
    {
        var command = new UpdateUserCommand(UserId, request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{UserId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(int UserId)
    {
        var command = new DeleteUserCommand(UserId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPost("[action]")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ActivateUser(int UserId)
    {
        var command = new ActivateUserCommand(UserId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPost("[action]")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeactivateUser(int UserId)
    {
        var command = new DeactivateUserCommand(UserId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("[action]")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllUser()
    {
        var query = new GetAllUserQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUserById(int UserId)
    {
        var query = new GetUserByIdQuery(UserId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}