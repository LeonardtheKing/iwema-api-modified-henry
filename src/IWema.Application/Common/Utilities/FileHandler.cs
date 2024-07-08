//using IWema.Application.Common.DTO;
//using Microsoft.AspNetCore.Http;

//namespace IWema.Application.Common.Utilities
//{
//    public static class FileHandler
//    {

//        private static string _folderPath = Path.Combine("wwwroot", "images");

//        public static async Task<ServiceResponse<string>> SaveFileAsync(IFormFile? file, CancellationToken cancellationToken = default)
//        {
//            try
//            {
//                if (file == null || file.Length == 0)
//                {
//                    return new("File is not selected or empty.");
//                }

//                var fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
//                var filePath = Path.Combine(_folderPath, fileName);

//                // Ensure the directory exists
//                var directoryPath = _folderPath;
//                if (!Directory.Exists(directoryPath))
//                {
//                    Directory.CreateDirectory(directoryPath);
//                }

//                // Save the file to the specified directory
//                using (var fileStream = new FileStream(filePath, FileMode.Create))
//                {
//                    await file.CopyToAsync(fileStream, cancellationToken);
//                }

//                return new("", true, fileName);
//            }
//            catch 
//            {
//                // Log the exception if needed
//                return new("Unable to Save File", false);
//            }
//        }


//        public static async Task<string> GetImageUrlAsync(IFormFile fileRef, IHttpContextAccessor httpContextAccessor)
//        {
//            try
//            {
//                if (!Directory.Exists(_folderPath))
//                    Directory.CreateDirectory(_folderPath);

//                var fileName = $"{Guid.NewGuid()}.{Path.GetExtension(fileRef.FileName).TrimStart('.')}";
//                var path = Path.Combine(_folderPath, fileName);

//                using var stream = fileRef.OpenReadStream();
//                using FileStream outputFileStream = new(path, FileMode.Create, FileAccess.Write);

//                await stream.CopyToAsync(outputFileStream);

//                var request = httpContextAccessor.HttpContext.Request;
//                //var request = httpRequest.HttpContext.Request;
//                var baseUrl = $"{request.Scheme}://{request.Host}";
//                var fileUrl = $"{baseUrl}/iwema-api/images/{fileName}";

//                return fileUrl;
//            }
//            catch (Exception)
//            {
//                return "An Error Occurred";
//            }
//        }

//        public static async Task<ServiceResponse> DeleteFileAsync(string fileName)
//        {
//            await Task.Delay(0);
//            try
//            {
//                var path = Path.Combine(_folderPath, fileName);
//                if (File.Exists(path))
//                {
//                    File.Delete(path);
//                    return new ServiceResponse("File deleted", true);
//                }
//                else
//                    return new ServiceResponse("File not found", false);
//            }
//            catch (Exception)
//            {
//                return new ServiceResponse("An error occurred.", false);
//            }
//        }

//        public static async Task<ServiceResponse> ReadFileAsync(string fileName, Stream outputStream)
//        {
//            var path = Path.Combine(_folderPath, fileName);
//            if (!File.Exists(path))
//            {
//                return new ServiceResponse("File not found.");
//            }

//            using FileStream fsSource = new(path, FileMode.Open, FileAccess.Read);
//            await fsSource.CopyToAsync(outputStream);

//            return new ServiceResponse("", true);
//        }

//        public static async Task<string> UpdateImageAsync(IFormFile fileRef,IHttpContextAccessor httpContextAccessor)
//        {
//            try
//            {
//                if (!Directory.Exists(_folderPath))
//                    Directory.CreateDirectory(_folderPath);

//                var newFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileRef.FileName).TrimStart('.')}";
//                var newFilePath = Path.Combine(_folderPath, newFileName);

//                var request = httpContextAccessor.HttpContext.Request;
//                var baseUrl = $"{request.Scheme}://{request.Host}";
//                var fileUrl = $"{baseUrl}/iwema-api/images/{newFileName}";

//                using (var stream = fileRef.OpenReadStream())
//                {
//                    using (FileStream outputFileStream = new(newFilePath, FileMode.Create, FileAccess.Write))
//                    {
//                        await stream.CopyToAsync(outputFileStream);
//                    }
//                }

//                return fileUrl;
//            }
//            catch (Exception)
//            {
//                return "Could not update file";
//            }
//        }

//        public static ServiceResponse<string> GetFullDirectoryLocation(HttpRequest request, string fileName)
//        {
//            try
//            {
//                var path = Path.Combine(_folderPath, fileName);
//                if (!File.Exists(path))
//                {
//                    return new("File not found.");
//                }

//                var baseUrl = $"{request.Scheme}://{request.Host}";
//                var fileUrl = $"{baseUrl}/iwema-api/images/{fileName}";
//                return new("", true, fileUrl);
//            }
//            catch (Exception)
//            {
//                return new("An Error Occurred", false);
//            }
//        }
//    }
//}

using IWema.Application.Common.DTO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace IWema.Application.Common.Utilities
{
    public static class FileHandler
    {
        public static async Task<string> SaveFileAsync(IFormFile? file, IWebHostEnvironment env, CancellationToken cancellationToken = default)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return "File is not selected or empty.";
                }

                //var folderPath = Path.Combine(env.WebRootPath, "images");
                var folderPath = Path.Combine(env.WebRootPath, "images");
                var fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var filePath = Path.Combine(folderPath, fileName);

                // Ensure the directory exists
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Save the file to the specified directory
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream, cancellationToken);
                }

                return fileName;
            }
            catch
            {
                // Log the exception if needed
                return "Unable to Save File";
            }
        }

        public static async Task<string> GetImageUrlAsync(IFormFile fileRef, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
        {
            try
            {
                var fileName = await SaveFileAsync(fileRef, env);

                if (fileName == "Unable to Save File" || fileName == "File is not selected or empty.")
                {
                    return "An Error Occurred";
                }

                var request = httpContextAccessor.HttpContext.Request;
                var baseUrl = $"{request.Scheme}://{request.Host}";
                var fileUrl = $"{baseUrl}/images/{fileName}";

                return fileUrl;
            }
            catch (Exception)
            {
                return "An Error Occurred";
            }
        }

        public static async Task<ServiceResponse> DeleteFileAsync(string fileName, IWebHostEnvironment env)
        {
            await Task.Delay(0);
            try
            {
                var folderPath = Path.Combine(env.WebRootPath, "images");
                var path = Path.Combine(folderPath, fileName);
                if (File.Exists(path))
                {
                    File.Delete(path);
                    return new ServiceResponse("File deleted", true);
                }
                else
                    return new ServiceResponse("File not found", false);
            }
            catch (Exception)
            {
                return new ServiceResponse("An error occurred.", false);
            }
        }

        public static async Task<ServiceResponse> ReadFileAsync(string fileName, Stream outputStream, IWebHostEnvironment env)
        {
            var folderPath = Path.Combine(env.WebRootPath, "images");
            var path = Path.Combine(folderPath, fileName);
            if (!File.Exists(path))
            {
                return new ServiceResponse("File not found.");
            }

            using FileStream fsSource = new(path, FileMode.Open, FileAccess.Read);
            await fsSource.CopyToAsync(outputStream);

            return new ServiceResponse("", true);
        }

        public static async Task<string> UpdateImageAsync(IFormFile fileRef, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
        {
            try
            {
                var fileName = await SaveFileAsync(fileRef, env);

                if (fileName == "Unable to Save File" || fileName == "File is not selected or empty.")
                {
                    return "Could not update file";
                }

                var request = httpContextAccessor.HttpContext.Request;
                var baseUrl = $"{request.Scheme}://{request.Host}";
                var fileUrl = $"{baseUrl}/iwema-api/images/{fileName}";

                return fileUrl;
            }
            catch (Exception)
            {
                return "Could not update file";
            }
        }

        public static ServiceResponse<string> GetFullDirectoryLocation(HttpRequest request, string fileName, IWebHostEnvironment env)
        {
            try
            {
                var folderPath = Path.Combine(env.WebRootPath, "images");
                var path = Path.Combine(folderPath, fileName);
                if (!File.Exists(path))
                {
                    return new("File not found.");
                }

                var baseUrl = $"{request.Scheme}://{request.Host}";
                var fileUrl = $"{baseUrl}/iwema-api/images/{fileName}";
                return new("", true, fileUrl);
            }
            catch (Exception)
            {
                return new("An Error Occurred", false);
            }
        }
    }
}

