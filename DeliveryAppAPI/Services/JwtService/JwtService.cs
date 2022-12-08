using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DeliveryAppAPI.Configurations;
using DeliveryAppAPI.DbContexts;
using DeliveryAppAPI.Helpers;
using DeliveryAppAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DeliveryAppAPI.Services.JwtService;

public class JwtService : IJwtService
{
    private readonly ApplicationDbContext _context;

    public JwtService(ApplicationDbContext context)
    {
        _context = context;
    }

    public string GetToken(ClaimsIdentity identity)
    {
        var now = DateTime.UtcNow;

        var jwtToken = new JwtSecurityToken(
            issuer: JwtConfigurations.Issuer,
            audience: JwtConfigurations.Audience,
            notBefore: now,
            claims: identity.Claims,
            expires: now.AddMinutes(JwtConfigurations.Lifetime),
            signingCredentials: new SigningCredentials(JwtConfigurations.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));
        
        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }

    public async Task<ClaimsIdentity?> GetIdentity(LoginCredentials credentials)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x =>
            x.Email == credentials.Email
        );

        if (user == null || !HashHelper.ValidatePassword(credentials.Password, user.Password))
        {
            return null;
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Hash, Guid.NewGuid().ToString())
        };

        var claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
        return claimsIdentity;
    }
}