using Microsoft.AspNetCore.Builder;
using MinistryData.WorkerService.Misc;
using MinistryData.WorkerService.Misc.DbContext;
using MinistryData.WorkerService.Misc.Interface;
using MinistryData.WorkerService.Misc.Repository;
using MinistryData.WorkerService.Misc.Service;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);
var conStr = new ConnectionString
{
    Server = "Data Source = (DESCRIPTION =  (ADDRESS_LIST =  (ADDRESS = (PROTOCOL = TCP)(HOST =119.40.95.187)(PORT = 1521)) )(CONNECT_DATA = (SID = dbjam))); User ID = miscbill; Password = miscbill",

};
Connection.Initialize(conStr);

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", true)
                .AddEnvironmentVariables()
                .Build();

var connectionString = configuration.GetConnectionString("DefaultConnection");

var migrationAssemblyName = typeof(Worker).Assembly.FullName;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

try
{
    Log.Information("Application Starting up");

    IHost host = Host.CreateDefaultBuilder(args)
        .UseWindowsService()
        .UseSerilog()
        .ConfigureServices(services =>
        {
            services.AddSingleton<IDapperService, DapperService>();
            services.AddSingleton<ICommonRepository, CommonRepository>();
            services.AddSingleton<IDatabaseConfigRepository, DatabaseConfigRepository>();
            services.AddSingleton<IInsertCustomerRepository, InsertCustomerRepository>();

            services.AddHostedService<Worker>();
        })
        .Build();

    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}