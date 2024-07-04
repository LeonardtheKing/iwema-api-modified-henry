using IWema.Application;
using IWema.Application.Common.Configuration;
using IWema.Infrastructure;
using IWema.Infrastructure.Adapters.SeamlessHR;

namespace IWema.Api.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.ConfigureOptions(configuration);
        services.ConfigureApplicationServices();
        services.ConfigureInfrastructureServices(configuration);

        return services;
    }

    public static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure<GeneralConfigOptions>(configuration.GetSection(nameof(GeneralConfigOptions)));
        services
           .Configure<SeamlessHRConfigOptions>(configuration.GetSection(nameof(SeamlessHRConfigOptions)));
        services
           .Configure<SecurityOptions>(configuration.GetSection(nameof(SecurityOptions)));
    }
}