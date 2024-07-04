using IWema.Api;
using IWema.Api.Extensions;
using IWema.Infrastructure;
using IWema.Infrastructure.Adapters.SeamlessHR;
using IWema.Infrastructure.Persistence.Seeding;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.ConfigureKestrelServer();

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
app.RemoveSensitiveHeaders();
app.SerilogConfiguration();

app.ConfigureApplication();
app.UseStaticFiles();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseRateLimiter();

// Call the SeedData method
await Seeder.SeedData(app.Services);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

