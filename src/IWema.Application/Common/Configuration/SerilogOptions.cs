namespace IWema.Api.Extensions;


using System.Collections.Generic;

    public class SerilogOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string TableName { get; set; } = string.Empty;
        public bool AutoCreateSqlTable { get; set; }
        public string FilePath { get; set; } = string.Empty;
    }

    public class Serilog
    {
        public List<string> Using { get; set; } = new List<string>();
        public MinimumLogLevel MinimumLevel { get; set; } = new MinimumLogLevel();
        public List<WriteToConfiguration> WriteTo { get; set; } = new List<WriteToConfiguration>();
        public List<string> Enrich { get; set; } = new List<string>();
    }

    public class MinimumLogLevel
    {
        public string Default { get; set; } = string.Empty;
        public Dictionary<string, string> Override { get; set; } = new Dictionary<string, string>();
    }

    public class WriteToConfiguration
    {
        public string Name { get; set; } = string.Empty;
        public WriteToArgs Args { get; set; } = new WriteToArgs();
    }

    public class WriteToArgs
    {
        public string Path { get; set; } = string.Empty;
        public string RollingInterval { get; set; } = string.Empty;
        public bool RollOnFileSizeLimit { get; set; }
        public string Formatter { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
        public string TableName { get; set; } = string.Empty;
        public bool AutoCreateSqlTable { get; set; }
    }

