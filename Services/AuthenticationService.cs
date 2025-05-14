using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using ReferBuddy.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Logging;

namespace ReferBuddy.Services;

public static class AuthenticationService
{
    public static IServiceCollection AddAuthenticationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Enable PII logging in development
        IdentityModelEventSource.ShowPII = true;

        var authConfig = configuration.GetSection("Auth").Get<AuthConfig>()
            ?? throw new InvalidOperationException("Auth configuration is missing");

        services.Configure<AuthConfig>(configuration.GetSection("Auth"));

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdConnect(options =>
            {
                // Auth0 settings
                options.Authority = authConfig.Authority;
                options.ClientId = authConfig.ClientId;
                options.ClientSecret = authConfig.ClientSecret;

                // Set response type to code
                options.ResponseType = OpenIdConnectResponseType.Code;

                // Configure the scopes
                options.Scope.Clear();
                foreach (var scope in authConfig.Scopes)
                {
                    options.Scope.Add(scope);
                }

                // Set callback path
                options.CallbackPath = authConfig.CallbackPath;
                options.SignedOutCallbackPath = authConfig.SignedOutCallbackPath;

                // Configure OIDC settings
                options.RequireHttpsMetadata = true;
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.UseTokenLifetime = true;

                // Auth0-specific configuration
                options.MetadataAddress = $"{authConfig.Authority}/.well-known/openid-configuration";

                // Configure token validation parameters
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "https://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = authConfig.Authority,
                    ValidAudience = authConfig.ClientId
                };

                // Configure Auth0 specific settings
                options.Events = new OpenIdConnectEvents
                {
                    OnTokenValidated = context =>
                    {
                        var logger = context.HttpContext.RequestServices
                            .GetRequiredService<ILogger<OpenIdConnectHandler>>();
                        logger.LogInformation("Token validation successful");
                        return Task.CompletedTask;
                    },

                    OnAuthenticationFailed = context =>
                    {
                        var logger = context.HttpContext.RequestServices
                            .GetRequiredService<ILogger<OpenIdConnectHandler>>();
                        logger.LogError(context.Exception, "Authentication failed");

                        // Don't expose the error details in production
                        if (context.HttpContext.RequestServices.GetService<IWebHostEnvironment>()?.IsDevelopment() ?? false)
                        {
                            context.Response.Redirect("/error?message=" + context.Exception.Message);
                            context.HandleResponse();
                        }

                        return Task.CompletedTask;
                    },

                    OnRedirectToIdentityProvider = context =>
                    {
                        // Don't set the audience parameter as it's not needed for the authorization endpoint
                        return Task.CompletedTask;
                    }
                };
            });

        return services;
    }
}
