﻿using IWema.Application.Common.DTO;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace IWema.Application.UpcomingEvents.Command.PartialUpdate;

public record PartiallyUpdateUpcomingEventsCommand(
       Guid Id,
       string? NameOfEvent = null,
       string? Date = null,
       IFormFile? File = null
   ) : IRequest<ServiceResponse>;