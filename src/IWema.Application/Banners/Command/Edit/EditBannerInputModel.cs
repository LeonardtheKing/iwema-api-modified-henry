using Microsoft.AspNetCore.Http;

namespace IWema.Application.Banners.Command.Edit;

public class EditBannerInputModel
{
    public Guid Id { get; set; }
    public IFormFile File { get; set; }
    public string Title { get; set; }
    public bool IsActive { get; set; }
}
