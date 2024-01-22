using Business.Cqrs;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Schemes.Dtos;
using Constants = Schemes.Constants.Constants;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpenseCategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExpenseCategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = Constants.Roles.Admin)]
    public async Task<IActionResult> CreateExpenseCategory([FromBody] CreateExpenseCategoryRequest request)
    {
        var command = new CreateExpenseCategoryCommand(request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{expenseCategoryId}")]
    [Authorize(Roles = Constants.Roles.Admin)]
    public async Task<IActionResult> UpdateExpenseCategory(int expenseCategoryId, [FromBody] UpdateExpenseCategoryRequest request)
    {
        var command = new UpdateExpenseCategoryCommand(expenseCategoryId, request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{expenseCategoryId}")]
    [Authorize(Roles = Constants.Roles.Admin)]
    public async Task<IActionResult> DeleteExpenseCategory(int expenseCategoryId)
    {
        var command = new DeleteExpenseCategoryCommand(expenseCategoryId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("[action]")]
    [Authorize(Roles = Constants.Roles.Admin)]
    public async Task<IActionResult> GetAllExpenseCategory()
    {
        var query = new GetAllExpenseCategoryQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet]
    [Authorize(Roles = Constants.Roles.Admin)]
    public async Task<IActionResult> GetExpenseCategoryById(int expenseCategoryId)
    {
        var query = new GetExpenseCategoryByIdQuery(expenseCategoryId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}