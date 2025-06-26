//using AdminSystem.API.Hubs;
using AdminSystem.Infrastructure.Persistence.Context;
using CFEMS.API.Configurations;
using CFEMS.Infrastructure.Common.Extensions;
using CFEMS.Infrastructure.Common.Middlewares;
using CFEMS.Infrastructure.Common.Versions;
using CFEMS.Infrastructure.DependencyInjection;
using CFEMS.Infrastructure.Mailing;
using Core.Application.DependencyInjection;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;

var builder = WebApplication.CreateBuilder(args);



builder.Host.AddConfigurations();
builder.Host.UseSerilog(builder.Configuration);
var conStr = new ConnectionString
{
    Server = builder.Configuration["DatabaseSettings:ConnectionString"],
    MiscTransfer = builder.Configuration["DatabaseSettings:ConnectionStringMISCBILLTRANS"],
    ConsumerBill = builder.Configuration["DatabaseSettings:ConnectionStringCONSBILLPAY"],
    ConsumerBillTranfer = builder.Configuration["DatabaseSettings:ConnectionStringCONSBILLDATATRANS"]
};
Connection.Initialize(conStr);

// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddApllicationServices();
builder.Services.AddAuthService(builder.Configuration);
builder.Services.AddRepositoryServices();
builder.Services.AddMailing(builder.Configuration);
builder.Services.AddApiVersioningExtension();
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<ForwardedHeadersOptions>(options => {
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});


//builder.Services.AddSerilogService(builder.Configuration);

Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Error()
                    .WriteTo.Oracle(cfg =>
                    cfg.WithSettings(builder.Configuration["DatabaseSettings:ConnectionString"])
                    .UseBurstBatch()
                    .CreateSink())
                    .CreateLogger();
Log.Information("Writing Logs in Oracle Database...");


var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) { }
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.EnableFilter();
    c.DisplayRequestDuration();
    //c.IndexStream = () => GetType().Assembly.GetManifestResourceStream("CustomUIIndex.Swagger.index.html");
    c.InjectStylesheet("/swagger-ui/custom.css");
    //c.InjectStylesheet(typeof(SwaggerConfig).Assembly,
    //  "ProjectName.FolderName.SwaggerHeader.css"));
});

app.UseCors("!infoNetPolicy");

app.UseMiddleware<ExceptionMiddleware>();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
