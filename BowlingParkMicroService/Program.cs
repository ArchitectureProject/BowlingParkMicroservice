using BowlingParkMicroService.Helpers;
using BowlingParkMicroService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NetDevPack.Security.JwtExtensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// cors
services.AddCors(options =>
{
    options.AddDefaultPolicy(corsPolicyBuilder =>
    {
        Console.Out.WriteLine("Adding cors policy");
        corsPolicyBuilder.WithOrigins("*")
            .AllowAnyHeader()
            .AllowAnyMethod(); 
    });
});

services.AddControllers(options =>
{
    options.Filters.Add<AppExceptionFiltersAttribute>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BowlingPark", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// HttpClients
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient("UserApi", httpClient =>
{
    httpClient.BaseAddress = new Uri(
        Environment.GetEnvironmentVariable("USERAPI_URL") ??
        "http://localhost:8080");
}).AddHttpMessageHandler<AuthHandler>();

// Services
services.AddScoped<IBowlingParkService, BowlingParkService>();
services.AddScoped<IUserApiService, UserApiService>();
services.AddScoped<ITokenProvider, TokenProvider>();
services.AddScoped<AuthHandler>();

// DbContext
services.AddDbContext<DataContext>(options =>
{
    var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
                           "Host=localhost:5432;Database=bowlingpark-bdd;Username=admin;Password=aupGjXqZCMh9vKkQ";
    options.UseNpgsql(connectionString);
});

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    // it shouldn't be that in production ofc (it should be true)
    options.RequireHttpsMetadata = false;
    options.SaveToken = false;
    options.IncludeErrorDetails = true;
    options.SetJwksOptions(new JwkOptions(
        (Environment.GetEnvironmentVariable("USERAPI_URL") ?? "http://localhost:8080") + "/public_key",
        "UserMicroservice"
    ));
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();