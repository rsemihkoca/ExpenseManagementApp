using Application.Cqrs;
using Business.Enums;
using Infrastructure.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ExpenseController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExpenseController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // Create Expense
    [HttpPost]
    [Authorize(Roles = "Admin") ]
    public async Task<IActionResult> CreateExpense([FromBody] InsertExpenseRequest request)
    {
        var command = new CreateExpenseCommand(request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    // Update Expense
    [HttpPut("{expenseRequestId}")]
    public async Task<IActionResult> UpdateExpense(int expenseRequestId, [FromBody] UpdateExpenseRequest request)
    {
        var command = new UpdateExpenseCommand(expenseRequestId, request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    // Delete Expense
    [HttpDelete("{expenseRequestId}")]
    public async Task<IActionResult> DeleteExpense(int expenseRequestId)
    {
        var command = new DeleteExpenseCommand(expenseRequestId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    // Get all Expenses
    [HttpGet]
    public async Task<IActionResult> GetAllExpenses()
    {
        var query = new GetAllExpenseQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    // Get Expense by Id
    [HttpGet("{expenseRequestId}")]
    public async Task<IActionResult> GetExpenseById(int expenseRequestId)
    {
        var query = new GetExpenseByIdQuery(expenseRequestId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
