using Serilog;

namespace Core.Application.DependencyInjection
{
    public static class AddSerilog
    {
        public static void AddSerilogService(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["DatabaseSettings:ConnectionString"];
            Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Oracle(cfg =>
                    cfg.WithSettings(connectionString)
                    .UseBurstBatch()
                    .CreateSink())
                    .CreateLogger();
            Log.Information("Writing Logs in Oracle Database...");

            //var h = new WebHostBuilder();
            //var environment = h.GetSetting("environment");

            //var config = new ConfigurationBuilder()
            //   .SetBasePath(Directory.GetCurrentDirectory())
            //   .AddJsonFile($"Configurations/database.{environment}.json", optional: true, reloadOnChange: true)
            //   .Build();

            //Log.Logger = new LoggerConfiguration()
            //    .ReadFrom.Configuration(configuration)
            //    .CreateLogger();

            //var connectionString = configuration["DatabaseSettings:ConnectionString"];

            //Log.Logger = new LoggerConfiguration()
            //    .MinimumLevel.Error()
            //    .WriteTo.Oracle(cfg =>
            //    cfg.WithSettings(connectionString)
            //    .UseBurstBatch()
            //    .CreateSink())
            //    .CreateLogger();

            try
            {
                Log.Information("Appliction strting up...");
            }
            catch (Exception ex)
            {
                Log.Fatal("The appliction failed to start correctly. Stcak trace: " + ex.ToString());
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

    }
}
