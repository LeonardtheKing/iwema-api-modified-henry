using FluentValidation;
using FluentValidation.AspNetCore;
using IWema.Application.Common.Behaviours;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace IWema.Application;

public static class ServiceExtension
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        // Register MediatR 
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        // Register Fluent Validation
        services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
            .AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // Register Pipeline behaviors
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehaviour<,>));

        services.AddMemoryCache();

        return services;
    }
}