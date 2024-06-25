using IWema.Application.Common.DTO;
using IWema.Application.Contract.SeamlessHR;
using IWema.Application.Contract.SeamlessHR.DTO;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Authentication;

namespace IWema.Infrastructure.Adapters.SeamlessHR.Services;

internal class ContactDirectoryService(IOptions<SeamlessHRConfigOptions> options) : IContactDirectoryService
{
    private readonly SeamlessHRConfigOptions _seamlessConfigOptions = options.Value;

    public async Task<ServiceResponse<List<ContactDirectoryResponseData>>> GetContactDirectories(string? searchTerm)
    {
        string secretKey = _seamlessConfigOptions.ApiKeyValue;

        HttpClientHandler clientHandler = new HttpClientHandler
        {
            UseDefaultCredentials = true,
            SslProtocols = SslProtocols.Tls12,
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        using var httpClient = new HttpClient(clientHandler);

        httpClient.DefaultRequestHeaders.Add("secret", secretKey);

        List<ContactDirectoryResponseData> filteredStaff = new List<ContactDirectoryResponseData>();
        string requestUrl = string.IsNullOrWhiteSpace(searchTerm) ?
            $"{_seamlessConfigOptions.Staff}all" :
            $"{_seamlessConfigOptions.Staff}find?q={searchTerm}";

        HttpResponseMessage response;

        try
        {
            response = await httpClient.GetAsync(requestUrl);
        }
        catch 
        {
            return new ServiceResponse<List<ContactDirectoryResponseData>>("Network error occurred while fetching staff");
        }

        if (!response.IsSuccessStatusCode)
        {
            return new ServiceResponse<List<ContactDirectoryResponseData>>("Unable to fetch staff");
        }

        string jsonResponse;

        try
        {
            jsonResponse = await response.Content.ReadAsStringAsync();
        }
        catch
        {

            return new ServiceResponse<List<ContactDirectoryResponseData>>("Error reading response from server");
        }

        ContactDirectoryResponse responseData;

        try
        {
            responseData = JsonConvert.DeserializeObject<ContactDirectoryResponse>(jsonResponse);
        }
        catch 
        {
            return new ServiceResponse<List<ContactDirectoryResponseData>>("Error deserializing server response");
        }

        if (responseData == null || responseData.Data == null)
        {
            return new ServiceResponse<List<ContactDirectoryResponseData>>("Invalid response data");
        }

        filteredStaff = responseData.Data;

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            filteredStaff = filteredStaff.Take(_seamlessConfigOptions.Limit).ToList();
        }

        return new ServiceResponse<List<ContactDirectoryResponseData>>("", true, filteredStaff);
    }
}
