using API.Extensions;
using API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration); // Register application-specific services like DbContext, CORS, etc.
builder.Services.AddIdentityServices(builder.Configuration); // Register identity-related services like authentication

var app = builder.Build();

// Use custom exception handling middleware to catch and handle exceptions globally
app.UseMiddleware<ExceptionMiddleware>();

// Configure CORS policy to allow requests from specific origins with any header or method
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:5000", "https://localhost:5001"));

// Enable authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
app.MapControllers();

app.Run();
