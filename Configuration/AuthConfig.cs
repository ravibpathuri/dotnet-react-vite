namespace ReferBuddy.Configuration;

public class AuthConfig
{
    public string Authority { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string[] Scopes { get; set; } = new[] { "openid", "profile", "email" };
    public string CallbackPath { get; set; } = "/signin-oidc";
    public string SignedOutCallbackPath { get; set; } = "/signout-callback-oidc";
    public string[] AllowedRedirectUris { get; set; } = Array.Empty<string>();
}
