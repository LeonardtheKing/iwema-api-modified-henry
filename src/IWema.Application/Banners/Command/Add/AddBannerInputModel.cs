using Microsoft.AspNetCore.Http;

namespace IWema.Application.Banners.Command.Add;

public class AddBannerInputModel
{
    public IFormFile File { get; set; }
    public string Title { get; set; }
    public bool IsActive { get; set; } = false;
}