using MinimalApi.Endpoint.Extensions;
using Serilog;
using Serilog.Events;
using Stash_And_Grab.Api.Startup;
using Stash_And_Grab.Application.Startup;
using Stash_And_Grab.Data.InMemory.Startup;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();


Log.Information("Starting web application");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.RegisterLibraryServices();
    builder.Services.RegisterInMemoryDataServices();
    builder.Services.RegisterServices();

    var app = builder.Build();

    app.ConfigureSwagger();

    app.UseHttpsRedirection();

    app.RegisterServiceEndpoints();

    app.MapEndpoints();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program
{
}