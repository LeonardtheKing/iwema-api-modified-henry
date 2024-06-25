using IWema.Application.MenuBars.Command.Add;
using IWema.Application.MenuBars.Command.Delete;
using IWema.Application.MenuBars.Command.Update;
using IWema.Application.MenuBars.Query.GetAll;
using IWema.Application.MenuBars.Query.GetById;
using IWema.Application.MenuBars.Query.GetFavourite;
using IWema.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IWema.Api.Controllers;

public class MenuBarController(IMediator mediator) : BaseController
{
    [Authorize(Roles = Role.ADMIN)]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await mediator.Send(new GetAllMenuBarQuery());
        return ServiceResponse(response);
    }

    [Authorize(Roles = Role.ADMIN)]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var response = await mediator.Send(new GetMenuBarByIdQuery(id));
        return ServiceResponse(response);
    }

    [Authorize(Roles = Role.ADMIN)]
    [HttpGet("favorite")]
    public async Task<IActionResult> GetFavorite()
    {
        var response = await mediator.Send(new GetFavoriteQuery());
        return ServiceResponse(response);
    }

    [Authorize(Roles = Role.ADMIN)]
    [HttpPost]
    public async Task<IActionResult> Add(AddMenuBarInputModel request)
    {
        AddMenuBarCommand command = new(request.Name, request.Link, request.Description, request.Icon, request.Hits);
        var response = await mediator.Send(command);

        return ServiceResponse(response);
    }
    
   [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateMenuBarInputModel request)
    {
        UpdateMenuBarCommand command = new(id, request.Id, request.Name, request.Link, request.Description, request.Icon);
        var updateResult = await mediator.Send(command);

        return ServiceResponse(updateResult);
    }

   [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPut("hit/{id}")]
    public async Task<IActionResult> UpdateHit(Guid id)
    {
        var response = await mediator.Send(new UpdateHitsCommand(id));
        return ServiceResponse(response);
    }

   [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await mediator.Send(new DeleteMenuBarCommand(id));

        return ServiceResponse(response);
    }

}