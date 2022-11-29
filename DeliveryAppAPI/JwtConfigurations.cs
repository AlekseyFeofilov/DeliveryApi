using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DeliveryAppAPI;

public static class JwtConfigurations
{
    public const string Issuer = "JwtTestIssuer"; 
    public const string Audience = "JwtTestClient";
    private const string Key = "SuperSecretKeyBazingaLolKek!*228322";  
    public const int Lifetime = 1440;
    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
    }
}
