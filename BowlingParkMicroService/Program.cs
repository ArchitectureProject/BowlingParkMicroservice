using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BowlingParkMicroService.Helpers;
using BowlingParkMicroService.Helpers.Auth;
using BowlingParkMicroService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
var domain = (Environment.GetEnvironmentVariable("DOMAIN") ?? "http://localhost:8080") + "/public_key";

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
    c.AddSignalRSwaggerGen();
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

// Services
services.AddScoped<IBowlingParkService, BowlingParkService>();
builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

// DbContext
services.AddDbContext<DataContext>(options =>
{
    var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
                           "Host=localhost:5432;Database=bowlingpark-bdd;Username=admin;Password=aupGjXqZCMh9vKkQ";
    options.UseNpgsql(connectionString);
});

// policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AGENT", policy =>
        policy.Requirements.Add(new HasScopeRequirement("read:messages", domain)));
    options.AddPolicy("CUSTOMER", 
        policy => policy.RequireClaim("role", "CUSTOMER"));
});

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = domain ?? "http://localhost:8080";
        options.Audience = "OtherMicroservices";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = ClaimTypes.NameIdentifier
        };
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