namespace IWema.Infrastructure;

public class SerilogOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;
    public bool AutoCreateSqlTable { get; set; }
    public string FilePath { get; set; } = string.Empty;
}

