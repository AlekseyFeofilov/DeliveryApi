using DeliveryAppAPI.Configurations;
using DeliveryAppAPI.DbContexts;
using DeliveryAppAPI.Middlewares;
using DeliveryAppAPI.ServiceExtensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefaultService();
builder.Services.AddSwaggerServices();

builder.Services.AddLogicServices();
builder.Services.AddHelperServices();

builder.AddRedisServices();

builder.Services.AddAuthorizationServices();
builder.Services.AddAuthenticationServices();

//DbContext 
var connection = builder.Configuration.GetConnectionString(ConnectionStrings.DefaultConnection);
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connection));
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

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