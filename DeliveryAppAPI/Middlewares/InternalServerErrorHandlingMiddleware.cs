using DeliveryAppAPI.Models.Response;
using Newtonsoft.Json;

namespace DeliveryAppAPI.Middlewares;

public class InternalServerErrorHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
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