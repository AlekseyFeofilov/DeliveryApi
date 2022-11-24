using System.Security.Claims;
using DeliveryAppAPI.Models;

namespace DeliveryAppAPI.Services.JwtService;

public interface IJwtService: IJwtClaimService
{
    string GetToken(ClaimsIdentity identity);
    Task<ClaimsIdentity?> GetIdentity(LoginCredentials credentials);
}