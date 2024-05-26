using CloudinaryDotNet;
using job_search_be.Api.Hubs;
using job_search_be.Api.Infrastructure.Extensions;
using job_search_be.Application.Helpers;
using job_search_be.Application.Module;
using job_search_be.Infrastructure.Context;
using job_search_be.Infrastructure.Exceptions;
using job_search_be.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationModules();

//Jwt
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    // Cấu hình bảo mật Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [Space] then your token"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
            new string[]{}
        }
        });
});
builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JWTSettings"));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.ConfigureAuth(builder.Configuration);
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddAuthorization();










//ConnectStrings
builder.Services.AddDbContext<job_search_DbContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("job_Context")));
// Configure SignalR
builder.Services.AddSignalR();
//Cloudinary
IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

var cloudinaryAccount = new Account(
    configuration["Cloudinary:CloudName"],
    configuration["Cloudinary:ApiKey"],
    configuration["Cloudinary:ApiSecret"]
);
var cloudinary = new Cloudinary(cloudinaryAccount);
builder.Services.AddSingleton(cloudinary);
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseHttpsRedirection();
app.UseFileServer();
app.UseStaticFiles();
app.UseCustomExceptionMiddleware();
app.UseAuthentication();
app.UseCors(builder =>
{
    builder
        .WithOrigins("http://localhost:5173", "https://job-search-five-flax.vercel.app")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials(); 
});

app.UseAuthorization();
app.MapControllers();
app.MapHub<NotificationHub>("/notificationHub");




app.Run();
