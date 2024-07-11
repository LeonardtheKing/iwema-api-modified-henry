using IWema.Application.Dashboard.ActiveUsers;
using IWema.Application.Dashboard.AverageVisitDuration;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IWema.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DashboardController(IMediator mediator) : BaseController
{
    [HttpGet("live-users")]
    public async Task<IActionResult> GetActiveUserLoginTimes()
    {

        var response = await mediator.Send(new GetActiveUserCountQuery());
        return ServiceResponse(response);

    }

    [HttpGet("average-visit-duration")]
    public async Task<IActionResult> GetAverageVisitDuration([FromQuery] DateTime? date, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {

        var query = new AverageVisitDurationQuery
        {
            Date = date,
            StartDate = startDate,
            EndDate = endDate
        };

        var response = await mediator.Send(query);
        return ServiceResponse(response);

        //if (response.Successful)
        //{
        //    return Ok(response);
        //}
        //else
        //{
        //    return BadRequest(response);
        //}
    }
}
