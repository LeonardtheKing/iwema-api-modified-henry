using IWema.Application.Libraries.Command.Add;
using IWema.Application.Libraries.Command.Delete;
using IWema.Application.Libraries.Query.GetAll;
using IWema.Application.Libraries.Query.GetById;
using IWema.Application.Libraries.Query.GetFilebyId;
using IWema.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IWema.Api.Controllers;
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class LibraryController(IMediator mediator) : BaseController
{
    [Authorize(Roles = Role.ADMIN)]
    [HttpPost]
    public async Task<IActionResult> Add(AddLibraryInputModel request)
    {
        AddLibraryCommand command = new(request.File, request.Description, request.Type);
        var response = await mediator.Send(command);
        return ServiceResponse(response);
    }

    [Authorize(Roles = Role.ADMIN)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        DeleteLibraryCommand command = new(id);
        var response = await mediator.Send(command);
        return ServiceResponse(response);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await mediator.Send(new GetAllLibraryQuery());
        return ServiceResponse(response); 
    }

    [HttpGet("file/{id}")]
    public async Task<IActionResult> GetFile(Guid id)
    {
        var response = await mediator.Send(new GetLibraryFileByIdQuery(id));
        
        if (response.Successful)
            return File(response.Response.MemoryStream, response.Response.Extension, response.Response.Name);
        else
            return BadRequest(response.Message);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await mediator.Send(new GetLibraryByIdQuery(id));
        return ServiceResponse(response);
    }

}
