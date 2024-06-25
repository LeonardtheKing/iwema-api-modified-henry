using Microsoft.AspNetCore.Http;

namespace IWema.Application.Libraries.Command.Add;

public class AddLibraryInputModel {
    public IFormFile File { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
}

