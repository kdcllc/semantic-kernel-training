using System.Reflection;

using Serilog;
using Serilog.Events;
using Serilog.Sinks.AzureAnalytics;
using CommandLine;

// Start our smart AppHost
AppHost.Start(args, Assembly.GetEntryAssembly()?.GetName().Name);

// parse the cli options
var options = Parser.Default.ParseArguments<CliOptions>(args);

// Create a Serilog Logger
Log.Logger = AppHost.CreateSerilogLogger(
    (logger, configuration, env) =>
    {
        if (!string.IsNullOrEmpty(configuration["AzureLogAnalytics:WorkspaceId"])
        && !string.IsNullOrEmpty(configuration["AzureLogAnalytics:AuthenticationId"]))
        {
            logger.WriteTo.AzureAnalytics(
                configuration["AzureLogAnalytics:WorkspaceId"],
                configuration["AzureLogAnalytics:AuthenticationId"],
                new ConfigurationSettings
                {
                    Flatten = false,
                    LogName = $"{env.ApplicationName}{env.EnvironmentName}",
                    BufferSize = 1,
                    BatchSize = 1
                },
                restrictedToMinimumLevel: LogEventLevel.Information);
        }
    });

try
{
    Log.Information("Starting AppHost");

    using var host = AppHost
                    .CreateHostBuilder()
                    .ConfigureAppConfiguration(x => x.AddUserSecrets<Program>())
                    .ConfigureServices(services =>
                    {
                        services.AddOptions<CliOptions>()
                                .Configure(o => o.Type = options.Value.Type);
                    })
                    .ConfigureServices(ConsoleServiceCollectionExtensions.ConfigureServices)
                    .Build();

    await host.StartAsync();

    var result = await host.ExecuteAsync(async m => await m.RunAsync());

    await host.StopAsync();

    Log.Information("AppHost Stopped");

    return result;
}
catch (Exception ex)
{
    Log.Fatal(ex, "AppHost terminated unexpectedly");

    return 1;
}
finally
{
    Log.CloseAndFlush();
}
