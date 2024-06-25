using IWema.Application.Common.DTO;
using Microsoft.AspNetCore.Mvc;

namespace IWema.Api.Controllers;

[Route("api/[controller]"), ApiController]
public abstract class BaseController : ControllerBase
{
    [NonAction] public IActionResult ServiceResponse<T>(ServiceResponse<T> serviceResponse) => Response(serviceResponse);

    [NonAction] public IActionResult ServiceResponse(ServiceResponse serviceResponse) => Response(serviceResponse);

    [NonAction]
    private new IActionResult Response(ServiceResponse serviceResponse)
    {
        switch (serviceResponse.Successful)
        {
            case true:
                return Ok(serviceResponse);
            case false:
                return BadRequest(serviceResponse);
        }
    }
}