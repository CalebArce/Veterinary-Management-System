using System.Net;
using System.Text.Json;
using VeterinarySystem.Application.Common;

namespace VeterinarySystem.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<ExceptionHandlingMiddleware> logger;

    public ExceptionHandlingMiddleware(RequestDelegate _next,ILogger<ExceptionHandlingMiddleware> _logger)
    {
        next = _next;
        logger = _logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error no controlado en la API");

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var response = ApiResponse<object>.Fail(ex.Message);

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
               PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
            });

            await httpContext.Response.WriteAsync(json);
        }
    }
}