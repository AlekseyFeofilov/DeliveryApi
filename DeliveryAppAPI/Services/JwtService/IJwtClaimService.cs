namespace DeliveryAppAPI.Services.JwtService;

public interface IJwtClaimService
{
    Guid GetIdClaim(HttpRequest request);
}