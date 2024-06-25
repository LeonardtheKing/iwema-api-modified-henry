using IWema.Application.ContactDirectory.Query.GetContactDirectory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IWema.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactDirectoryController(IMediator mediator) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetContactDirectory(
            [FromQuery] string? searchTerm)
        {
            var query = new GetContactDirectoryQuery(searchTerm);

            var response = await mediator.Send(query);
             return ServiceResponse(response);      
        }
    }
}

