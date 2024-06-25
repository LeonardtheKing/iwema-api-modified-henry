using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace IWema.Application.Common.Utilities;

public class FileHandler : IFileHandler
{

    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly string _folderPath;
    private readonly IHttpContextAccessor _httpContextAccessor;



    public FileHandler(IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
    {

        _hostingEnvironment = hostingEnvironment;
        _folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, "images");
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResponse<string>> SaveFile(IFormFile fileRef)
    {

        if (!Directory.Exists(_folderPath))
            Directory.CreateDirectory(_folderPath);

            var fileName = $"{Guid.NewGuid()}.{Path.GetExtension(fileRef.FileName)}";
            var path = Path.Combine(_folderPath, fileName);

            using var stream = fileRef.OpenReadStream();
            using FileStream outputFileStream = new(path, FileMode.Create, FileAccess.Write);

            await stream.CopyToAsync(outputFileStream);

            return new("", true, fileName);
              
    }


    public async Task<string> GetImageUrl(IFormFile fileRef)
    {
        try
        {
            if (!Directory.Exists(_folderPath))
                Directory.CreateDirectory(_folderPath);

            var fileName = $"{Guid.NewGuid()}.{Path.GetExtension(fileRef.FileName).TrimStart('.')}"; // Ensure extension is properly formatted
            var path = Path.Combine(_folderPath, fileName);

            using var stream = fileRef.OpenReadStream();
            using FileStream outputFileStream = new(path, FileMode.Create, FileAccess.Write);

            await stream.CopyToAsync(outputFileStream);

            // Construct URL
            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            var fileUrl = $"{baseUrl}/iwema-api/images/{fileName}"; // Adjust the path based on how your files are accessible

            return fileUrl;
        }
        catch (Exception)
        {
            return "An Error Occured";
        }
    }



    public async Task<ServiceResponse> DeleteFileAsync(string fileName)
    {
        await Task.Delay(0);
        try
        {
            var path = Path.Combine(_folderPath, fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
                return new("File deleted", true);
            }
            else
                return new("File not found", false);
        }
        catch (Exception)
        {
            return new("An error occurred.", false);
        }
    }

    public async Task<ServiceResponse> ReadFile(string fileName, Stream outputStream)
    {
        var path = Path.Combine(_folderPath, fileName);
        if (!File.Exists(path))
        {
            return new("File not found.");
        }

        using FileStream fsSource = new(path, FileMode.Open, FileAccess.Read);
        await fsSource.CopyToAsync(outputStream);

        return new("", true);
    }



    public async Task<string> UpdateImage(IFormFile fileRef)
    {
        try
        {
            if (!Directory.Exists(_folderPath))
                Directory.CreateDirectory(_folderPath);

            // Generate a new unique filename
            var newFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileRef.FileName)}";
            var newFilePath = Path.Combine(_folderPath, newFileName);

            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            var fileUrl = $"{baseUrl}/iwema/images/{newFileName}"; // Adjust the path based on how your files are accessible

            // Save the new file
            using (var stream = fileRef.OpenReadStream())
            {
                using (FileStream outputFileStream = new(newFilePath, FileMode.Create, FileAccess.Write))
                {
                    await stream.CopyToAsync(outputFileStream);
                }
            }

            return fileUrl;
        }
        catch (Exception)
        {
            return "Could  not Update File";
        }
    }

    //public  ServiceResponse<string> GetFullDirectoryLocation(HttpRequest request, string fileName)
    //{
    //    var path = Path.Combine(folderPath, fileName);
    //    if (!File.Exists(path))
    //    {
    //        return new("File not found.");
    //    }

    //    var baseUrl = $"{request.Scheme}://{request.Host}";
    //    var fileUrl = $"{baseUrl}/iwema-api/upload/images/{fileName}";
    //    return new("", true, fileUrl);
    //}

    public ServiceResponse<string> GetFullDirectoryLocation(HttpRequest request, string fileName)
    {
        try
        {
            var path = Path.Combine(_folderPath, fileName); // Ensure _folderPath is properly referenced
            if (!File.Exists(path))
            {
                return new ServiceResponse<string>("File not found.");
            }

            var baseUrl = $"{request.Scheme}://{request.Host}";
            var fileUrl = $"{baseUrl}/iwema-api/images/{fileName}";
            return new ServiceResponse<string>("", true, fileUrl);
        }
        catch (Exception)
        {
            return new ServiceResponse<string>("An Error Occurred", false);
        }
    }



}
