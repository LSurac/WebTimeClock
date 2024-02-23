using System.Text;
using Application;
using ApplicationData;
using ApplicationNotification;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using SqlDataAccess;
using WebTimeClock.Configuration;

var builder = WebApplication.CreateBuilder(args);
IHostEnvironment env = builder.Environment;
builder.Configuration.Sources.Clear();
builder.Configuration
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);
var configuration = (IConfiguration)builder.Configuration;
var tokenConfigurationSection = configuration.GetSection(TokenSettings.SectionName);
var tokenConfiguration = tokenConfigurationSection.Get<TokenSettings>();


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument();

builder.Services.AddDbServices(configuration);
builder.Services.AddApplicationNotification(configuration);
builder.Services.AddApplicationData();
builder.Services.AddApplication();


builder.Services.AddAuthentication(
    opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(
    options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "http://localhost:5000",
            ValidAudience = "http://localhost:5000",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfiguration.TokenIssuerSigningKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddSwaggerDocument(
    config =>
    {
        config.PostProcess = document =>
        {
            document.Info.Version = "v1";
            document.Info.Title = "Web Time Clock API";
            document.Info.Description = "ASP.NET Web API for Web Time Clock.";

            document.Info.Contact = new OpenApiContact
            {
                Name = "Leonard Surac",
                Email = "leonardsurac@web.de"
            };
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseOpenApi();

app.MapControllers();

app.Run();