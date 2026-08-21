using Microsoft.EntityFrameworkCore;
using Serilog;
using API.Controllers;
using Application.InPorts;
using Application.OutPorts;
using Application.Services;
using Storage.Context;
using Storage.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(o =>
        o.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Warning)
    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddSingleton(Log.Logger);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("PostgresConnection")
    ));

builder.Services.AddScoped<IOpportunityRepo, OpportunityRepo>();
builder.Services.AddScoped<IOpportunityReadRepo, OpportunityReadRepo>();
builder.Services.AddScoped<IOpportunityCommands, OpportunityCommandService>();
builder.Services.AddScoped<IOpportunityQueries, OpportunityQueryService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSerilogRequestLogging();
app.UseSwagger(options =>
{
    options.RouteTemplate = "api/swagger/{documentName}/swagger.json";
});
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/api/swagger/v1/swagger.json", "API V1");
    options.RoutePrefix = "api/swagger";
});
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();
app.MapGet("api/health", () => Results.Ok("Healthy"));
app.Run();