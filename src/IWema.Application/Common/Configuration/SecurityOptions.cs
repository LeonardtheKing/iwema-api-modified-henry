namespace IWema.Application.Common.Configuration;

public class SecurityOptions
{
    public string[] HeadersToRemove { get; set; } = [];
    public HeaderToAppend[] HeadersToAppend { get; set; } = [];
}

public class HeaderToAppend
{
    public string Header { get; set; }=string.Empty;
    public string Value { get; set; }=string.Empty;
}

