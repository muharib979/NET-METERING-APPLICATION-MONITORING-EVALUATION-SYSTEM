using CFEMS.SignalR.Extensions;
using CFEMS.SignalR.Hubs.Alarm;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(builder.Configuration);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSignalR();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()){   }

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins(builder.Configuration["CorsSettings:Angular"], builder.Configuration["CorsSettings:CFEMSAPI"]));

//app.UseAuthorization();

app.MapControllers();
app.MapHub<AlarmsHub>("hubs/alarms");

app.Run();
