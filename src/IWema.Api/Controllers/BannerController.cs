using IWema.Application.Banners.Command.Add;
using IWema.Application.Banners.Command.Delete;
using IWema.Application.Banners.Command.Edit;
using IWema.Application.Banners.Query.GetAll;
using IWema.Application.Banners.Query.GetById;
using IWema.Application.Banners.Query.GetFileById;
using IWema.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IWema.Api.Controllers;

[Authorize]
public class BannerController(IMediator mediator) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Add(AddBannerInputModel request)
    {
        AddBannerCommand command = new(request.File, request.Title, request.IsActive);
        var response = await mediator.Send(command);
        return ServiceResponse(response);
    }

  
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        DeleteBannerCommand command = new(id);
        var response = await mediator.Send(command);
        return ServiceResponse(response);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await mediator.Send(new GetAllBannerQuery());
        return ServiceResponse(response);
    }

    [HttpGet("file/{id}")]
    public async Task<IActionResult> GetMedia(Guid id)
    {
        var response = await mediator.Send(new GetBannerFileByIdQuery(id));
        if (response.Successful)
            return File(response.Response.MemoryStream, response.Response.Extension);
        else
            return BadRequest(response.Message);
    }

    [Authorize(Roles = Role.ADMIN)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await mediator.Send(new GetBannerByIdQuery(id));
        return ServiceResponse(response);
    }

 
    [HttpPost("edit")]
    public async Task<IActionResult> EditBanner(EditBannerInputModel model)
    {
        EditBannerCommand command = new(model.Id, model.File, model.Title, model.IsActive);
        var response = await mediator.Send(command);
        return ServiceResponse(response);
    }
}
