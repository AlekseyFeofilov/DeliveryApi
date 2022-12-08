using System.Security.Claims;
using DeliveryAppAPI.AuthorizationPolicies.AuthorizationRequirements;
using DeliveryAppAPI.Configurations;
using DeliveryAppAPI.Exceptions;
using DeliveryAppAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;

namespace DeliveryAppAPI.AuthorizationPolicies.AuthorizationHandlers;

[Authorize]
public class ActiveTokenHandler : AuthorizationHandler<ActiveTokenRequirement>
{
    private readonly IDistributedCache _redis;

    public ActiveTokenHandler(IDistributedCache redis)
    {
        _redis = redis;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        ActiveTokenRequirement requirement)
    {
        var tokenId = context.User.FindFirst(ClaimTypes.Hash)?.Value;
        if (tokenId == null) throw new UnauthorizedException();

        if (await _redis.GetRecordAsync(AppConfigurations.InstanceName + tokenId) == null)
        {
            context.Succeed(requirement);
        }
        else
        {
            throw new UnauthorizedException();
        }
    }
}