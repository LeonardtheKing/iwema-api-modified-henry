namespace IWema.Api.Extensions;

public static class ApplicationExtension
{
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        app.MigrateApplication();
        return app;
    }

    public async static void MigrateApplication(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            //var context = services.GetRequiredService<IWemaDbContext>();
            //await context.Database.MigrateAsync();
            //await DataInitializer.InitializeUsersAndRolesAsync(app.Services);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred during migration");
        }

    }
}