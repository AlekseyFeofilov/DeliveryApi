using DeliveryAppAPI.Exceptions;
using DeliveryAppAPI.Models.Response;
using Newtonsoft.Json;

namespace DeliveryAppAPI.Middlewares;

public class ErrorHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (UnauthorizedException)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
        catch (NotFoundException)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;   
        }
        catch (Exception exception)
        {
            context.Response.ContentType = "application/json"; 
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var response = JsonConvert.SerializeObject(new Response("Unexpected error", exception.ToString()));

            await context.Response.WriteAsync(response);
        }
    }
}