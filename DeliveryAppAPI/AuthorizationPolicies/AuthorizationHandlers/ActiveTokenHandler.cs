using DeliveryAppAPI.AuthorizationPolicies.AuthorizationRequirements;
using DeliveryAppAPI.Configurations;
using DeliveryAppAPI.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;

namespace DeliveryAppAPI.AuthorizationPolicies.AuthorizationHandlers;

public class ActiveTokenHandler : AuthorizationHandler<ActiveTokenRequirement>
{
    private readonly IDistributedCache _redis;
    private readonly IHttpContextAccessor _contextAccessor;

    public ActiveTokenHandler(IHttpContextAccessor contextAccessor, IDistributedCache redis)
    {
        _contextAccessor = contextAccessor;
        _redis = redis;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        ActiveTokenRequirement requirement)
    {
        string? authHeader = _contextAccessor.HttpContext?.Request.Headers[StringConstance.Authorization];

        if (authHeader == null) throw new UnauthorizedException();

        if (await _redis.GetRecordAsync<string>(AppConfigurations.InstanceName + authHeader) == null)
        {
            context.Succeed(requirement);
        }
        else
        {
            throw new UnauthorizedException();
        }
    }
}