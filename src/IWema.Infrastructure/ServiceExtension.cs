using IWema.Application.Common.Configuration;
using IWema.Application.Contract;
using IWema.Application.Contract.SeamlessHR;
using IWema.Domain.Entity;
using IWema.Infrastructure.Adapters;
using IWema.Infrastructure.Adapters.ActiveDirectory;
using IWema.Infrastructure.Adapters.ActiveDirectory.Services;
using IWema.Infrastructure.Adapters.SeamlessHR.Services;
using IWema.Infrastructure.Caching;
using IWema.Infrastructure.Persistence;
using IWema.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Threading.RateLimiting;

namespace IWema.Infrastructure;

public static class ServiceExtension
{
    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Bind configuration sections to classes
        services.Configure<JwtConfigOptions>(configuration.GetSection("JWT"));
        services.Configure<DatabaseSettings>(configuration.GetSection("ConnectionStrings"));
        services.Configure<RateLimiting>(configuration.GetSection("RateLimiting"));



        services.Configure<ActiveDirectoryConfigOptions>(configuration.GetSection("ActiveDirectory"));

        // Register the database
        services.AddDbContext<IWemaDbContext>((serviceProvider, options) =>
        {
            var databaseSettings = serviceProvider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            options.UseSqlServer(databaseSettings.IWemaDbContext);
        });

        // Register Repositories
        services
            .AddScoped<IMenuBarRepository, MenuBarRepository>()
            .AddScoped<IManagementTeamRepository, ManagementTeamRepository>()
            .AddScoped<INewsRepository, NewsRepository>()
            .AddScoped<IBannerRepository, BannerRepository>()
            .AddScoped<ISideMenuRepository, SideMenuRepository>()
            .AddScoped<ICachingAdapter, CachingAdapter>()
            .AddScoped<IBirthDayService, BirthdayService>()
            .AddScoped<IUpcomingEventsRepository, UpcomingEventsRepository>()
            .AddScoped<IAnniversaryService, AnniversaryService>()
            .AddScoped<IAnnouncementRepository, AnnouncementRepository>()
            .AddScoped<ILibraryRepository, LibraryRepository>()
            .AddScoped<IContactDirectoryService, ContactDirectoryService>()
            .AddScoped<IJwtTokenManager, JwtTokenManager>()
            .AddScoped<IActiveDirectoryService, ActiveDirectoryService>()
            .AddScoped<IApplicationUserRepository, ApplicationUserRepository>()
            .AddScoped<IBlogRepository, BlogRepository>();
     

        services.AddHttpClient("iwema", client =>
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            UseDefaultCredentials = true,
            SslProtocols = SslProtocols.Tls12,
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        });


        //Add  Rate Limiter
        services.AddRateLimiter(options =>
        {
            var rateLimiting = services.BuildServiceProvider().GetRequiredService<IOptions<RateLimiting>>().Value;
            options.AddFixedWindowLimiter("FixedWindowPolicy", opt =>
            {
                opt.Window = TimeSpan.FromSeconds(rateLimiting.WindowTimeSpan);
                opt.PermitLimit = rateLimiting.PermitLimit;
                opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                opt.QueueLimit = rateLimiting.QueueLimit;
            }).RejectionStatusCode = rateLimiting.RejectionStatusCode;

        });

        services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
        {
            //previous code removed for clarity reasons
            opt.Lockout.AllowedForNewUsers = true;
            opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(30);
            opt.Lockout.MaxFailedAccessAttempts = 4;
        }).AddEntityFrameworkStores<IWemaDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    public static IServiceCollection AddIdentityAuthenticationServices(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var jwtSettings = serviceProvider.GetRequiredService<IOptions<JwtConfigOptions>>().Value;

        services.AddHttpContextAccessor();

        return services;
    }
}
