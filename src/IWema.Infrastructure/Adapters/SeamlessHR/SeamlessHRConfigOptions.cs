namespace IWema.Infrastructure.Adapters.SeamlessHR;

public class SeamlessHRConfigOptions
{
    public string Birthdays { get; set; } = string.Empty;
    public string Anniversaries { get; set; } = string.Empty;
    public string Staff { get; set; } = string.Empty;
    public string ApiKeyValue { get; set; } = string.Empty;
    public int Limit { get; set; }
    public string Status { get; set; } = string.Empty;
}
