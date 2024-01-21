using Application.Cqrs;
using Infrastructure.Dtos;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentInstructionController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentInstructionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreatePaymentInstruction([FromBody] CreatePaymentInstructionRequest request)
    {
        var command = new CreatePaymentInstructionCommand(request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{paymentInstructionId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdatePaymentInstruction(int paymentInstructionId, [FromBody] UpdatePaymentInstructionRequest request)
    {
        var command = new UpdatePaymentInstructionCommand(paymentInstructionId, request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{paymentInstructionId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeletePaymentInstruction(int paymentInstructionId)
    {
        var command = new DeletePaymentInstructionCommand(paymentInstructionId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    

    [HttpGet("[action]")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllPaymentInstruction()
    {
        var query = new GetAllPaymentInstructionQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetPaymentInstructionById(int paymentInstructionId)
    {
        var query = new GetPaymentInstructionByIdQuery(paymentInstructionId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}