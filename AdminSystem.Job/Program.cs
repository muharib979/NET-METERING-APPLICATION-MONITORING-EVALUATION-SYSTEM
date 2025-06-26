using AdminSystem.Job.Dependency;
using AdminSystem.Job.Jobs;
using AdminSystem.Job.Models;
using Quartz;
using static Dapper.SqlMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApllicationServices();
var settings = builder.Configuration.GetSection("CornSetting").Get<CronSettings>();
builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionScopedJobFactory();
    var jobKey = new JobKey("CustomerJob");
    q.AddJob<CustomerJob>(opts => opts.WithIdentity(jobKey));
    var tiggerkey = new TriggerKey("trigger1");

    q.AddTrigger(opts => opts
    .ForJob(jobKey)
    .WithIdentity(tiggerkey)
    .WithCronSchedule(settings.CustomerInsertCronExpression));
});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseStaticFiles();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
