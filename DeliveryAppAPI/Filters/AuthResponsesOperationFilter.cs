using DeliveryAppAPI.Configurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DeliveryAppAPI.Filters;

/// <inheritdoc />
// ReSharper disable once ClassNeverInstantiated.Global
public class AuthResponsesOperationFilter : IOperationFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.MethodInfo.DeclaringType == null) return;
        
        if (context.MethodInfo.GetCustomAttributes(true).Any(x => x is AuthorizeAttribute)
            || context.MethodInfo.DeclaringType.GetCustomAttributes(true).Any(x => x is AuthorizeAttribute))
        {
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = AppConfigurations.TokenType
                            }
                        },
                        Array.Empty<string>()
                    }
                }
            };
        }
    }
}