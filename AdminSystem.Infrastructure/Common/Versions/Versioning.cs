using Microsoft.AspNetCore.Mvc;

namespace CFEMS.Infrastructure.Common.Versions;

public static class Versioning
{
    public static void AddApiVersioningExtension(this IServiceCollection services)
    {
        services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(1, 0); // Specify the default API Version as 1.0        
            config.AssumeDefaultVersionWhenUnspecified = true; // If the client hasn't specified the API version in the request, use the default API version number 
            config.ReportApiVersions = true; // Advertise the API versions supported for the particular endpoint
        });
    }
}
