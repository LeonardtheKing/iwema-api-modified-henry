namespace IWema.Application.Common.Configuration;

public class JwtConfigOptions
{
    public string ValidIssuer { get; set; } = string.Empty;
    public string ValidAudience { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
    public string TokenExpiryInMinutes { get; set; } = string.Empty;

}
