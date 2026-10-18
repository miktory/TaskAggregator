using System.Reflection;
using System.Text;
using Mapster;
using Scalar.AspNetCore;
using Serilog;
using TaskAggregator.API.Extensions;
using TaskAggregator.Infrastructure.EfCore;
using TaskAggregator.Infrastructure.EfCore.Utilities;


// bootstrap logger
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/bootstrap.log")
    .CreateBootstrapLogger();

try
{
    Log.Logger.Information("Application initialization started.");
    var builder = WebApplication.CreateBuilder(args);
    var config = builder.Configuration;
    var services = builder.Services;


    // CONFIGURE SERVICES AREA

    // MAPSTER
    TypeAdapterConfig.GlobalSettings.Default.IgnoreNonMapped(true);
    TypeAdapterConfig.GlobalSettings.RequireExplicitMapping = true;
    TypeAdapterConfig.GlobalSettings.RequireExplicitMappingPrimitive = false;
    TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
    TypeAdapterConfig.GlobalSettings.Compile();

    // ADD LEGACY ENCODINGS SUPPORT
    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

    services.AddDomain(config);
    services.AddApplication(config);
    services.AddInfrastructure(config);
    services.AddPresentation(config);



    // CONFIGURE AREA
    var app = builder.Build();
    var env = app.Environment;

    // APPLY MIGRATIONS
    using (var scope = app.Services.CreateScope())
    {
        Log.Logger.Information("Database initialization started.");
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await DatabaseMigrationAssistant.InitDatabaseAsync(db, CancellationToken.None);
        Log.Logger.Information("Database initialization finished.");
    }

    if (env.IsDevelopment())
    {
        app.MapOpenApi().WithDocumentPerVersion();
        app.MapScalarApiReference();
    }
    // It's important that the localization middleware is added 
    // before the exception handling middleware; otherwise, you won't see the messages in the appropriate language.
    app.UseRequestLocalization();
    // Serilog request logging middleware must be added before exception handler. Otherwise handled exceptions will provide 
    // log records with 500 status code instead of fact code.
    app.UseSerilogRequestLogging();
    app.UseExceptionHandler();
    app.UseStatusCodePages();
    app.UseStaticFiles();
    app.UseRouting();
    app.MapHealthChecks("/status");
    //app.UseHttpsRedirection();
    app.UseCors("AllowAll");
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapDefaultControllerRoute();

    LoggingExtensions.LogBaseAppInfo(app.Logger);
    app.Run();
}
catch (Exception ex)
{
    Environment.ExitCode = 1;
    Log.Fatal(ex, "FATAL Application error!");
}
finally
{
    Log.CloseAndFlush();
}
