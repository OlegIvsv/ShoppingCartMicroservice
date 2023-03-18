namespace ShoppingCart.Infrastructure.Auth;

public class AuthSettings
{
    public const string SectionName = "Auth";
    public const string IdClaimName = "registered-id";
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience {get; set; }
}