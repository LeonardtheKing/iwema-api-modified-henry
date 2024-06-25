using IWema.Application.Common.DTO;
using Microsoft.AspNetCore.Http;

namespace IWema.Application.Common.Utilities;

public static class  FileHandler
{
    
   private readonly static string _folderPath = @"..\upload\images\";
    private readonly static string _imagePath = @"C:\Users\Izuchukwu.Okorie\Desktop\I-WEMA-SAMUEL\iwema-api\src\Files\Images";

    public static async Task<ServiceResponse<string>> SaveFile(IFormFile fileRef)
    {
        try
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
        catch (Exception ex)
        {
            return new("An error occured");
        }

    }

    public static async Task<ServiceResponse<string>> GetImageUrl(IFormFile fileRef)
    {
        try
        {
            if (!Directory.Exists(_imagePath))
                Directory.CreateDirectory(_folderPath);

            var fileName = $"{Guid.NewGuid()}.{Path.GetExtension(fileRef.FileName)}";
            var path = Path.Combine(_imagePath, fileName);

            using var stream = fileRef.OpenReadStream();
            using FileStream outputFileStream = new(path, FileMode.Create, FileAccess.Write);

            await stream.CopyToAsync(outputFileStream);

            return new ServiceResponse<string>("Image saved successfully", true, path);
        }
        catch (Exception ex)
        {
            return new ServiceResponse<string>($"An error occurred: {ex.Message}");
        }

    }

    public static async Task<ServiceResponse> DeleteFileAsync(string fileName)
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
        catch (Exception e)
        {
            return new("An error occurred.", false);
        }
    }

    public static async Task<ServiceResponse> ReadFile(string fileName, Stream outputStream)
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

  

    public static async Task<string> UpdateImage(IFormFile fileRef)
    {
        try
        {
            if (!Directory.Exists(_folderPath))
                Directory.CreateDirectory(_folderPath);

            // Generate a new unique filename
            var newFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileRef.FileName)}";
            var newFilePath = Path.Combine(_imagePath, newFileName);

            // Save the new file
            using (var stream = fileRef.OpenReadStream())
            {
                using (FileStream outputFileStream = new(newFilePath, FileMode.Create, FileAccess.Write))
                {
                    await stream.CopyToAsync(outputFileStream);
                }
            }

            return newFilePath;
        }
        catch (Exception ex)
        {
            return "Could  not Update File";
        }
    }



}
