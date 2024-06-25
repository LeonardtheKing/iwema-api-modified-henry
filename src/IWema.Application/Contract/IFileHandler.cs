using IWema.Application.Common.DTO;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace IWema.Application.Contract;

public interface IFileHandler
{
    Task<ServiceResponse<string>> SaveFile(IFormFile fileRef);
    Task<string> GetImageUrl(IFormFile fileRef);
    Task<ServiceResponse> DeleteFileAsync(string fileName);
    Task<ServiceResponse> ReadFile(string fileName, Stream outputStream);
    Task<string> UpdateImage(IFormFile fileRef);
    ServiceResponse<string> GetFullDirectoryLocation(HttpRequest request, string fileName);


}