using Core.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using Serilog;

namespace CFEMS.Infrastructure.Common.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            
            _logger.LogError(error, error.Message);
            Log.Fatal(error, error.Message);
            var response = context.Response;
            response.ContentType = "application/json";

            switch (error.InnerException)
            {
                case AppException e:
                    // custom application error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case KeyNotFoundException e:
                    // not found error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    // unhandled error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var result = JsonSerializer.Serialize(new { message = error?.InnerException?.Message?? error?.Message });
            await response.WriteAsync(result);

            //context.Response.ContentType = "application/json";

            //context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            //var response = _env.IsDevelopment()
            //    ? new ApiException(context.Response.StatusCode, ex.InnerException == null ? ex.Message : ex.InnerException.Message, ex.StackTrace?.ToString())
            //    : new ApiException(context.Response.StatusCode, "Internal Server Error");

            //var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            //var json = JsonSerializer.Serialize(response, options);

            //await context.Response.WriteAsync(json);
        }
    }
}
