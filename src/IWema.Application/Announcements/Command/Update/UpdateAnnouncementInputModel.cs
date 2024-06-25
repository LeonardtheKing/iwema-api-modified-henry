﻿using Microsoft.AspNetCore.Http;

namespace IWema.Application.Announcements.Command.Update;

public class UpdateAnnouncementInputModel
{
    public Guid Id { get; set; }
    public IFormFile File { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;


}
