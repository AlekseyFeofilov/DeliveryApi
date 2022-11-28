using DeliveryAppAPI.Models.Dto;

namespace DeliveryAppAPI.Services.JwtService;

public interface IJwtClaimService
{
    string GetClaimValue(string claimType, HttpRequest request);
}