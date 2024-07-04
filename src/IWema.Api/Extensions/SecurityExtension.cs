using IWema.Application.Common.Configuration;

namespace IWema.Api.Extensions;

public static class SecurityExtension
{
    public static void RemoveSensitiveHeaders(this WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            context.Response.OnStarting(() =>
            {
                var securityOptions = app.Configuration.GetSection(nameof(SecurityOptions)).Get<SecurityOptions>()!;

                foreach (var header in securityOptions.HeadersToAppend)
                    context.Response.Headers.Append(header.Header, header.Value);

                foreach (var header in securityOptions.HeadersToRemove)
                    context.Response.Headers.Remove(header);

                return Task.CompletedTask;
            });

            await next();
        });
    }

    public static void ConfigureKestrelServer(this WebApplicationBuilder builder)
    {
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.AddServerHeader = false;
        });
    }
}