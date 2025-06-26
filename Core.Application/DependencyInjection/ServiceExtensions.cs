using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("!infoNetPolicy", builder =>

            //builder.WithOrigins("http://localhost:4200", "https://infonetlimited.com", "http://27.147.216.162:80","http://119.40.95.187:1001")
            //.AllowAnyHeader()
            //.AllowAnyMethod()
            //.AllowCredentials()
            //    );
            //});

            builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
                );
            });
        }

        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {
            });
        }

    }
}
