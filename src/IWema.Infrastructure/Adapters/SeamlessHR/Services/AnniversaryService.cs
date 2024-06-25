
using IWema.Application.Common.DTO;
using IWema.Application.Contract.SeamlessHR;
using IWema.Application.Contract.SeamlessHR.DTO;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Security.Authentication;

namespace IWema.Infrastructure.Adapters.SeamlessHR.Services;

public class AnniversaryService(IOptions<SeamlessHRConfigOptions> options) : IAnniversaryService
{
    private readonly SeamlessHRConfigOptions _options = options.Value;

    public async Task<ServiceResponse<AnniversaryResponse>> GetAnniversaries()
    {
        try
        {
            string requestUrl = _options.Anniversaries + "?filterBy=month";
            string secretKey = _options.ApiKeyValue;

            // Create HttpClientHandler with SSL configuration
            HttpClientHandler clientHandler = new HttpClientHandler
            {
                UseDefaultCredentials = true,
                SslProtocols = SslProtocols.Tls12,
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            // Create HttpClient with custom handler
            var httpClient = new HttpClient(clientHandler);

            httpClient.DefaultRequestHeaders.Add("secret", secretKey);

            HttpResponseMessage response = await httpClient.GetAsync(requestUrl);
            if (!response.IsSuccessStatusCode)
            {
                return new("Unable to fetch anniversaries");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AnniversaryResponse>(jsonResponse);

            return new("", true, result);
        }
        catch (Exception ex)
        {
            return new("An error occurred while fetching anniversaries");
        }
    }
}


