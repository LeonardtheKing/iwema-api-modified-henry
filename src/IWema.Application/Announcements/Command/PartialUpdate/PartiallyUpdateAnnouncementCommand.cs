using IWema.Application.Common.DTO;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace IWema.Application.Announcements.Command.PartialUpdate;

public record PartiallyUpdateAnnouncementCommand(
       Guid Id,
       string? Title = null,
       string? Date = null,
       IFormFile? File = null,
       string? Content = null,
       string? Link=null
   ) : IRequest<ServiceResponse>;