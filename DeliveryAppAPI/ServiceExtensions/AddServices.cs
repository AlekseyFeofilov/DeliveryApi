using System.Reflection;
using DeliveryAppAPI.AuthorizationPolicies.AuthorizationHandlers;
using DeliveryAppAPI.AuthorizationPolicies.AuthorizationRequirements;
using DeliveryAppAPI.Configurations;
using DeliveryAppAPI.Filters;
using DeliveryAppAPI.Middlewares;
using DeliveryAppAPI.Profiles;
using DeliveryAppAPI.Services.BasketService;
using DeliveryAppAPI.Services.DishServices;
using DeliveryAppAPI.Services.JwtService;
using DeliveryAppAPI.Services.OrderService;
using DeliveryAppAPI.Services.RepositoryService;
using DeliveryAppAPI.Services.UserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

namespace DeliveryAppAPI.ServiceExtensions;

public static class AddServices 
{
    public static void AddLogicServices(this IServiceCollection services)
    {
        services.AddScoped<IDishBasketService, DishBasketService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IDishService, DishServices>();
        services.AddScoped<IOrderService, OrderService>();
        
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IRepositoryService, RepositoryService>();
        
        services.AddTransient<ErrorHandlingMiddleware>();
    }

    public static void AddSwaggerServices(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            options.OperationFilter<AuthResponsesOperationFilter>();
        });
    }

    public static void AddDefaultService(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
    }
    
    public static void AddRedisServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString(StringConstance.Redis);
            options.InstanceName = AppConfigurations.InstanceName;
        });

        builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(StringConstance.Localhost));
        
        builder.Services.AddDistributedMemoryCache();
        //builder.Services.AddSingleton<IDistributedCache, RedisCache>();
    }

    public static void AddHelperServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DbSetsProfile));
    }
    
    public static void AddAuthorizationServices(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, ActiveTokenHandler>();
        
        services.AddAuthorization();
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AppConfigurations.ActiveTokenPolicy, policy => policy.Requirements.Add(new ActiveTokenRequirement()));
        });
    }
    
    public static void AddAuthenticationServices(this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = JwtConfigurations.Issuer,
                    ValidateAudience = true,
                    ValidAudience = JwtConfigurations.Audience,
                    ValidateLifetime = true,
                    IssuerSigningKey = JwtConfigurations.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true
                };
            });
    }
}