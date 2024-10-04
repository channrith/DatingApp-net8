using System;
using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
  // Extension method to add application-specific services to the service collection
  public static IServiceCollection AddApplicationServices(this IServiceCollection services,
  IConfiguration config)
  {
    services.AddControllers(); // Register controllers for handling HTTP requests
    services.AddDbContext<DataContext>(opt =>
    {
      // Configure the database context to use SQLite with the connection string from the configuration
      opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
    });

    // Register CORS to allow cross-origin requests
    services.AddCors();
    // Register a scoped service for token generation
    services.AddScoped<ITokenService, TokenService>();
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IPhotoService, PhotoService>();
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

    return services;
  }
}
