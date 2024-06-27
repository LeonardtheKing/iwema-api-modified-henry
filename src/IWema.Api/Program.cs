using IWema.Api;
using IWema.Api.Extensions;
using IWema.Application.Common.Utilities;
using IWema.Infrastructure;
using IWema.Infrastructure.Adapters.SeamlessHR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

// Add Serilog configuration
// Bind Serilog options
var serilogOptions = builder.Configuration.GetSection("SerilogOptions").Get<SerilogOptions>();

// Add services to the container.
builder.Host.UseSerilog((context, configuration) =>
{
    var columnOptions = new ColumnOptions(); // Optional: Customize columns
    columnOptions.Store.Add(StandardColumn.LogEvent); // Optional: Add the LogEvent column

    var sinkOptions = new MSSqlServerSinkOptions
    {
        TableName = serilogOptions.TableName,
        AutoCreateSqlTable = serilogOptions.AutoCreateSqlTable
    };

    configuration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.File(serilogOptions.FilePath, rollingInterval: RollingInterval.Day)
        .WriteTo.MSSqlServer(
            connectionString: serilogOptions.ConnectionString,
            sinkOptions: sinkOptions,
            columnOptions: columnOptions
        );
});

builder.Services
  .AddOptions<SeamlessHRConfigOptions>()
  .Bind(builder.Configuration.GetSection("Seamless-HR"));
builder.Services.AddControllers();
builder.Services.AddSwaggerDocumentation();
builder.Services.ConfigureServices(builder.Configuration);

builder.Services.AddIdentityAuthenticationServices();
builder.Services.AddCors();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
        };
    });


builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));
var app = builder.Build();
app.UseCors(options =>
{
    options.AllowAnyOrigin(); // Allow requests from any origin
    options.AllowAnyMethod(); // Allow all HTTP methods
    options.AllowAnyHeader(); // Allow all headers
});
app.UseMiddleware<ExceptionHandlingMiddleware>();


app.ConfigureApplication();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "images")),
    RequestPath = "/images"
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseRateLimiter();

//var scope = app.Services.CreateScope();
//var context = scope.ServiceProvider.GetRequiredService<IWemaDbContext>();
//var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
//var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//await DataInitializer.InitializeUsersAndRolesAsync(context, userManager, roleManager);

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseMiddleware<CustomSecurityHeader>();
app.Run();

