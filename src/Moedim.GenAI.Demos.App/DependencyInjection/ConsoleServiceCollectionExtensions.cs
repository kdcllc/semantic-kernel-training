namespace Microsoft.Extensions.DependencyInjection;

public static class ConsoleServiceCollectionExtensions
{
    public static void ConfigureServices(HostBuilderContext hostBuilder, IServiceCollection services)
    {
        services.AddScoped<IMain, Main>();

        // this only needs to be added once.
        services.AddOpenAIOptions();
        
        // the other services are added in the Main class
        services.AddBasicDemoKernel();
    }
}
