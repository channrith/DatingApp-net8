using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions;

public static class IdentityServiceExtensions
{
  // Extension method to add JWT authentication to the service collection
  public static IServiceCollection AddIdentityServices(this IServiceCollection services,
  IConfiguration config)
  {
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
      // Retrieve the token key from the configuration
      var tokenKey = config["TokenKey"] ?? throw new Exception("TokenKey not found");
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuerSigningKey = true, // Validate the signing key for the token
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)), // Define the signing key
        ValidateIssuer = false, // Disable issuer validation
        ValidateAudience = false // Disable audience validation
      };
    });

    return services;
  }
}
