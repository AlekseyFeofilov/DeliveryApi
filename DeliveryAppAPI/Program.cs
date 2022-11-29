using DeliveryAppAPI;
using DeliveryAppAPI.DbContexts;
using DeliveryAppAPI.Services.BasketService;
using DeliveryAppAPI.Services.DishServices;
using DeliveryAppAPI.Services.JwtService;
using DeliveryAppAPI.Services.OrderService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using DeliveryAppAPI.Services.UserService;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IJwtClaimService, JwtService>();
builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDishService, DishServices>();
builder.Services.AddScoped<IOrderService, OrderService>();

//JWT
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

//DbContext 
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connection));
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true); //todo solve more smart

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();