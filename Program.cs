using ReferBuddy.Services;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

// Enable detailed error messages in development
if (builder.Environment.IsDevelopment())
{
    IdentityModelEventSource.ShowPII = true;
    builder.Logging.AddDebug();
    builder.Logging.AddConsole();
    builder.Logging.SetMinimumLevel(LogLevel.Debug);
}

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Add authentication
builder.Services.AddAuthenticationServices(builder.Configuration);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

else
{
    app.UseHttpsRedirection();
}

// In production, serve the built React app from wwwroot
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseCors();

// Add authentication middleware
app.UseAuthentication();
app.UseAuthorization();

// Map controllers with authorization requirement
app.MapControllers().RequireAuthorization();

// Allow anonymous access to auth endpoints
app.MapControllerRoute(
    name: "auth",
    pattern: "api/auth/{action}",
    defaults: new { controller = "Auth" }
).AllowAnonymous();

// In production, serve the SPA for any unmatched routes
if (!app.Environment.IsDevelopment())
{
    app.MapFallbackToFile("index.html");
}

app.Run();
