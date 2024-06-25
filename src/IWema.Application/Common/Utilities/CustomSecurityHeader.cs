using Microsoft.AspNetCore.Http;

namespace IWema.Application.Common.Utilities;


public class CustomSecurityHeader
{
    private readonly RequestDelegate _next;

    public CustomSecurityHeader(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        context.Response.Headers.Append("X-Frame-Options", "DENY");

        context.Response.Headers.Append("X-Permitted-Cross-Domain-Policies", "none");

        context.Response.Headers.Append("X-Xss-Protection", "1; mode=block");

        context.Response.Headers.Append("X-Content-Type-Options", "nosniff");

        context.Response.Headers.Append("Referrer-Policy", "no-referrer");


        context.Response.Headers.Append("Content-Security-Policy",
            "default-src 'self'");

        await _next.Invoke(context);
    }
}
