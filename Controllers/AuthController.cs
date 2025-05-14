using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ReferBuddy.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public AuthController(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    [HttpGet("login")]
    public IActionResult Login(string returnUrl = "/")
    {
        if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
        {
            returnUrl = "/";  // Default redirect to root if returnUrl is empty or not local
        }

        // In development, redirect to React dev server
        if (_environment.IsDevelopment())
        {
            returnUrl = $"http://localhost:5173{returnUrl}";
        }

        return Challenge(
            new AuthenticationProperties
            {
                RedirectUri = returnUrl,
                IsPersistent = true // This will make the authentication cookie persistent
            },
            "OpenIdConnect"
        );
    }

    [Authorize]
    [HttpGet("user")]
    public IActionResult GetUser()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value ?? User.FindFirst("email")?.Value;
        var name = User.FindFirst(ClaimTypes.Name)?.Value ?? User.Identity?.Name;
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return Ok(new
        {
            id,
            name,
            email
        });
    }

    [Authorize]
    [HttpGet("logout")]
    public IActionResult Logout()
    {
        return SignOut(
            new AuthenticationProperties { RedirectUri = "/" },
            CookieAuthenticationDefaults.AuthenticationScheme,
            "OpenIdConnect"
        );
    }
}
