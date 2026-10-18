using System.Reflection;
using System.Runtime.InteropServices;

namespace TaskAggregator.API.Extensions;

public static class LoggingExtensions
{
    public static void LogBaseAppInfo(ILogger logger)
    {
        logger.LogInformation(new string('-', 80));
        logger.LogInformation(@"  
█████  ███   ████ █   █     ███   ███   ███  ████  █████  ███   ███  █████  ███  ████    
 ░█░░░█ ░░█ █ ░░░░█░ █ ░   █ ░░█ █ ░░░ █ ░░░ █░░░█ █░░░░░█ ░░░ █ ░░█  ░█░░░█ ░░█ █░░░█   
  █░░░█████░ ███░░███ ░ ░  █████░█░ ██░█░ ██░████░░████░░█░ ██░█████░  █░░░█░ ░█░████░░  
  █░░ █░░░█░░ ░░█ █░░█ ░   █░░░█░█░░ █░█░░ █░█░░█░ █░░░░ █░░ █░█░░░█░░ █░░ █░░ █░█░░█░ ░ 
  █░░ █░░░█░████░░█░░░█    █░░░█░░███ ░░███ ░█░░░█░█████░ ███ ░█░░░█░░ █░░  ███ ░█░░░█░  
   ░░  ░░  ░░░░░░ ░░░  ░    ░░  ░░ ░░░ ░ ░░░ ░░░  ░ ░░░░░  ░░░ ░░░  ░░  ░░   ░░░ ░░░  ░  
    ░   ░   ░ ░░░░  ░   ░    ░   ░  ░░░   ░░░  ░   ░ ░░░░░  ░░░  ░   ░   ░    ░░░  ░   ░                                                                                                                                                                                                                                                                                          
    ");
        var ver = $"{Assembly.GetExecutingAssembly().GetName().Version}";
        var infoVer = Assembly.GetExecutingAssembly()
                                 .GetCustomAttribute<AssemblyInformationalVersionAttribute>()!
                                 .InformationalVersion;
        logger.LogDebug($"INTERNAL VERSION NAME: TaskAggregator {ver}");
        logger.LogInformation($"PUBLIC VERSION NAME: TaskAggregator {infoVer}");
        logger.LogInformation($"Started at: {DateTime.UtcNow} (UTC)");
        logger.LogInformation($"Environment: {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}");
        logger.LogInformation($"OS: {RuntimeInformation.OSDescription}");
        logger.LogInformation($"Process ID: {Environment.ProcessId}");
        logger.LogInformation(new string('-', 80));
    }
}
