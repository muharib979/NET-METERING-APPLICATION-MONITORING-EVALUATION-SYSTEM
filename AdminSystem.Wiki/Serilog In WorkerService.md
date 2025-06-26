# #Serilog In WorkerService

- Required Nuget Packages:

 ```
Serilog.Extensions.Hosting    ---- Serilog Package for WorkerService
Serilog.Settings.Configuration --- Used for getting Serilog Configuration from appsetting.json
Serilog.Sinks.Console ------------ Used for showing log in console
Serilog.Sinks.File --------------- Used for writing log in files
Serilog.Enrichers.Thread --------- Used for enriching Serilog log events with Thread Number
Serilog.Enrichers.Process -------- Used for enriching Serilog log events with Process Id 
Serilog.Enrichers.Environment----- Used for enriching Serilog log events with properties from System.Environment
 ```

- Serilog Configuration Settings in appsetting.Development.json:

```
  "Serilog": {
    "Using": [],
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning",
        "Microsoft.Hosting.Lifetime": "Information",
        "Microsoft.EntityFrameworkCore": "Warning"
      }
    },
    "Enrich": [ "FromLogContext", "WithMachineName", "WithProcessId", "WithThreadId" ],
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "File",
        "Args": {
          "path": "Logs/logs.json",
          "formatter": "Serilog.Formatting.Json.JsonFormatter, Serilog",
          "rollingInterval": "Day",
          "retainedFileCountLimit": 7
        }
      }
    ]
  }
}
```

- UseSerilog Extensions Method: 

```
namespace RBEProcessor.Extensions
{
    public static class SerilogExtension
    {
        public static IHostBuilder UseSerilogExtension(this IHostBuilder builder, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
            builder.UseSerilog();
            return builder;
        }
    }
}
```

- UseSerilogExtension Method using with host builder in Program.cs 

```
IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<IConfiguration>(config);
        services.AddRegisteredServices(config);
        services.AddHostedService<RBEHostedService>();
    })
    .UseSerilogExtension(config)
    .Build();

await host.RunAsync();

```
