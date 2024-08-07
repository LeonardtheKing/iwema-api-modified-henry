﻿namespace IWema.Application.MenuBars.Command.Update;

public class UpdateMenuBarInputModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}