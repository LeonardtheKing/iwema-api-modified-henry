namespace IWema.Infrastructure.Adapters.ActiveDirectory;

public class ActiveDirectoryConfigOptions
{
    public string Domain { get; set; } = string.Empty;
    public string LDapServerIP { get; set; } = string.Empty;
    public int LDapServerPort;

}
