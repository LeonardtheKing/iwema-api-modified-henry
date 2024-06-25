using IWema.Application.Common.DTO;
using IWema.Application.Contract.SeamlessHR;
using IWema.Application.Contract.SeamlessHR.DTO;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Security.Authentication;

namespace IWema.Infrastructure.Adapters.SeamlessHR.Services;

internal class BirthdayService( IOptions<SeamlessHRConfigOptions> options) : IBirthDayService
{
    private readonly SeamlessHRConfigOptions _seamlessConfigOptions = options.Value;

    public async Task<ServiceResponse<BirthdayResponse>> GetBirthdayCelebrants()
    {
        try
        {

            // Combine base URL and endpoint
            string requestUrl = _seamlessConfigOptions.Birthdays + "?filterBy=month";

            // Secret key
            string secretKey = _seamlessConfigOptions.ApiKeyValue;

            // Create HttpClientHandler with SSL configuration
            HttpClientHandler clientHandler = new HttpClientHandler
            {
                UseDefaultCredentials = true,
                SslProtocols = SslProtocols.Tls12,
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            // Create HttpClient with custom handler
            var httpClient = new HttpClient(clientHandler);

            // Add secret key to request headers
            httpClient.DefaultRequestHeaders.Add("secret", secretKey);

            // Make GET request to fetch birthday data
            HttpResponseMessage response = await httpClient.GetAsync(requestUrl);
            if (!response.IsSuccessStatusCode)
            {
                return new("Unable to fetch anniversaries");
            }

            // Read response content as string
            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Deserialize JSON response into the provided model format
            var result = JsonConvert.DeserializeObject<BirthdayResponse>(jsonResponse);

            return new("", true, result);
        }
        catch 
        {          
            return new("An error occurred while fetching anniversaries");
        }
    }


}

