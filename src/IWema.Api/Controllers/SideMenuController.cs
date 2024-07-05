using IWema.Application.SideMenu.ChildSideMenu.Command.Add;
using IWema.Application.SideMenu.ChildSideMenu.Command.Delete;
using IWema.Application.SideMenu.ChildSideMenu.Command.Update;
using IWema.Application.SideMenu.ChildSideMenu.Query.GetAllChildSideMenu;
using IWema.Application.SideMenu.ParentSideMenu.Command.Add;
using IWema.Application.SideMenu.ParentSideMenu.Command.Delete;
using IWema.Application.SideMenu.ParentSideMenu.Command.Update;
using IWema.Application.SideMenu.ParentSideMenu.Query.GetAllParentSideMenu;
using IWema.Application.SideMenu.ParentSideMenu.Query.GetParentSideMenu;
using IWema.Application.SideMenu.ParentSideMenu.Query.GetParentsWithChildren;
using IWema.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IWema.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class SideMenuController(IMediator mediator) : BaseController
    {
        [HttpPost("parentSideMenu")]
        public async Task<IActionResult> Add(AddParentSideMenuInputModel request)
        {
            AddParentSideMenuCommand Ccommand = new(request.Name, request.Icon);
            var response = await mediator.Send(Ccommand);
            return ServiceResponse(response);
        }

        [HttpPost("childSideMenu")]
        public async Task<IActionResult> AddChildSideMenu(AddChildSideMenuInputModel request)
        {
            AddChildSideMenuCommand Ccommand = new(request.ParentSideMenuId, request.Name, request.Link, request.Icon);
            var response = await mediator.Send(Ccommand);
            return ServiceResponse(response);
        }

        [HttpGet("parentSideMenu")]
        public async Task<IActionResult> GetParentSideMenu(Guid ParentSideMenuId)
        {
            var response = await mediator.Send(new GetParentSideMenuByIdQuery(ParentSideMenuId));
            return ServiceResponse(response);
        }

        [HttpDelete("parentMenu/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await mediator.Send(new DeleteParentToggleCommand(id));
            return ServiceResponse(response);
        }

        [Authorize(Roles = Role.ADMIN)]
        [HttpDelete("chilSidedMenu/{id}")]
        public async Task<IActionResult> DeleteChildSideMenu(Guid id)
        {
            var response = await mediator.Send(new DeleteChildToggleCommand(id));
            return ServiceResponse(response);
        }


        [HttpPut("parentSideMenu/{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateParentSideMenuInputModel request)
        {
            UpdateParentSideMenuCommand command = new(id, request.Id, request.Name, request.Icon);
            var updateResult = await mediator.Send(command);

            return ServiceResponse(updateResult);
        }

        [HttpPut("childSideMenu/{id}")]
        public async Task<IActionResult> UpdateChildSideMenu(Guid id, UpdateChildSideMenuInputModel request)
        {
            UpdateChildSideMenuCommand command = new(id, request.ParentToggleId, request.Icon, request.Name, request.Link);
            var updateResult = await mediator.Send(command);

            return ServiceResponse(updateResult);
        }

       
        [HttpGet("allParentSideMenu")]
        public async Task<IActionResult> GetAllParentSideMenu()
        {
            var response = await mediator.Send(new GetAllParentSideMenuQuery());
            return ServiceResponse(response);
        }

    
        [HttpGet("allChildSideMenu")]
        public async Task<IActionResult> GetAllChildMenu()
        {
            var response = await mediator.Send(new GetAllChildSideMenuQuery());
            return ServiceResponse(response);
        }

        [HttpGet("parentswithchildren")]
        public async Task<IActionResult> GetAllParentsWithChildren()
        {
            var response = await mediator.Send(new GetParentsWithChildrenQuery());
            return ServiceResponse(response);
        }
    }
}

