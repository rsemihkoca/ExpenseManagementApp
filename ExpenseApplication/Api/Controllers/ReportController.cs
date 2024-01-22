using Business.Cqrs;
using Infrastructure.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ReportController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("[action]")]
    [Authorize(Roles = Constants.Roles.Admin)]
    public async Task<IActionResult> ApprovedPaymentFrequencyReport([FromBody] ReportFrequencyRequest request)
    {
        var command = new ApprovedPaymentFrequencyReport(request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPost("[action]")]
    [Authorize(Roles = Constants.Roles.Admin)]
    public async Task<IActionResult> RejectedPaymentFrequencyReport([FromBody] ReportFrequencyRequest request)
    {
        var command = new RejectedPaymentFrequencyReport(request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPost("[action]")]
    [Authorize(Roles = Constants.Roles.Admin)]
    public async Task<IActionResult> PersonnelExpenseFrequencyReport([FromBody] ReportFrequencyRequest request)
    {
        var command = new PersonnelExpenseFrequencyReport(request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPost("[action]")]
    [Authorize(Roles = Constants.Roles.AdminOrPersonnel)]
    public async Task<IActionResult> PersonnelSummaryReport([FromBody] PersonnelSummaryRequest request)
    {
        var command = new PersonnelSummaryReport(request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

}