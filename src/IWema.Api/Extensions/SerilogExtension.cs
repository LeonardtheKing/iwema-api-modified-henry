using IWema.Application.Common.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Compact;
using Serilog.Sinks.MSSqlServer;

namespace IWema.Api.Extensions
{
    public static class SerilogExtension
    {
        public static void SerilogConfiguration(this WebApplication app)
        {
            var configuration = app.Configuration;

            var serilogOptions = configuration.GetSection(nameof(SerilogOptions)).Get<SerilogOptions>()!;
            var serilogConfigurations = configuration.GetSection(nameof(SERILOG)).Get<SERILOG>()!;

            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Is(GetLogEventLevel(serilogConfigurations.MinimumLevel.Default));

            foreach (var overridePair in serilogConfigurations.MinimumLevel.Override)
            {
                loggerConfiguration.MinimumLevel.Override(overridePair.Key, GetLogEventLevel(overridePair.Value));
            }

            foreach (var enrich in serilogConfigurations.Enrich)
            {
                switch (enrich)
                {
                    case "FromLogContext":
                        loggerConfiguration.Enrich.FromLogContext();
                        break;
                    case "WithMachineName":
                        loggerConfiguration.Enrich.WithMachineName();
                        break;
                    case "WithThreadId":
                        loggerConfiguration.Enrich.WithThreadId();
                        break;
                    // Add other enrichers as needed
                    default:
                        break;
                }
            }

            foreach (var sink in serilogConfigurations.Using)
            {
                switch (sink)
                {
                    case "Serilog.Sinks.Console":
                        loggerConfiguration.WriteTo.Console();
                        break;
                    case "Serilog.Sinks.File":
                        loggerConfiguration.WriteTo.File(
                            path: serilogOptions.FilePath,
                            rollingInterval: RollingInterval.Day,
                            rollOnFileSizeLimit: true,
                            formatter: new CompactJsonFormatter()
                        );
                        break;
                    case "Serilog.Sinks.MSSqlServer":
                        loggerConfiguration.WriteTo.MSSqlServer(
                            connectionString: serilogOptions.ConnectionString,
                            sinkOptions: new MSSqlServerSinkOptions { TableName = serilogOptions.TableName, AutoCreateSqlTable = serilogOptions.AutoCreateSqlTable }
                        );
                        break;
                    // Add other Serilog sinks here as needed
                    default:
                        break;
                }
            }

            foreach (var writeTo in serilogConfigurations.WriteTo)
            {
                switch (writeTo.Name)
                {
                    case "Console":
                        loggerConfiguration.WriteTo.Console();
                        break;
                    case "File":
                        ITextFormatter formatter = null;
                        if (!string.IsNullOrEmpty(writeTo.Args.Formatter) && writeTo.Args.Formatter == "Serilog.Formatting.Compact.CompactJsonFormatter, Serilog.Formatting.Compact")
                        {
                            formatter = new CompactJsonFormatter();
                        }

                        if (formatter != null)
                        {
                            loggerConfiguration.WriteTo.File(
                                path: writeTo.Args.Path,
                                rollingInterval: Enum.Parse<RollingInterval>(writeTo.Args.RollingInterval, true),
                                rollOnFileSizeLimit: writeTo.Args.RollOnFileSizeLimit,
                                formatter: formatter
                            );
                        }
                        else
                        {
                            loggerConfiguration.WriteTo.File(
                                path: writeTo.Args.Path,
                                rollingInterval: Enum.Parse<RollingInterval>(writeTo.Args.RollingInterval, true),
                                rollOnFileSizeLimit: writeTo.Args.RollOnFileSizeLimit
                            );
                        }
                        break;
                    case "MSSqlServer":
                        loggerConfiguration.WriteTo.MSSqlServer(
                            connectionString: writeTo.Args.ConnectionString,
                            sinkOptions: new MSSqlServerSinkOptions { TableName = writeTo.Args.TableName, AutoCreateSqlTable = writeTo.Args.AutoCreateSqlTable }
                        );
                        break;
                    // Add other Serilog sinks here as needed
                    default:
                        break;
                }
            }

            Log.Logger = loggerConfiguration.CreateLogger();

            app.UseSerilogRequestLogging(); // Optional: logs HTTP request information

            app.Use(async (context, next) =>
            {
                await next();
            });
        }

        private static LogEventLevel GetLogEventLevel(string level)
        {
            return level.ToLower() switch
            {
                "verbose" => LogEventLevel.Verbose,
                "debug" => LogEventLevel.Debug,
                "information" => LogEventLevel.Information,
                "warning" => LogEventLevel.Warning,
                "error" => LogEventLevel.Error,
                "fatal" => LogEventLevel.Fatal,
                _ => LogEventLevel.Information,
            };
        }
    }
}
