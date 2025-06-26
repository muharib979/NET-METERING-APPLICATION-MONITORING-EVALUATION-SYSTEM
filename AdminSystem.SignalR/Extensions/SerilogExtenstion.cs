using Serilog;

namespace CFEMS.SignalR.Extensions;

public static class SerilogExtenstion
{
    public static IHostBuilder UseSerilog(this IHostBuilder builder,IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
        builder.UseSerilog();
        return builder;
    }
}
