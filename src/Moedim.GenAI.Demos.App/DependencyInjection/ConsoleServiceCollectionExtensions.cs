using Moedim.GenAI.Demos.BasicDemos;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConsoleServiceCollectionExtensions
{
    public static void ConfigureServices(HostBuilderContext hostBuilder, IServiceCollection services)
    {
        services.AddScoped<IMain, Main>();

        // the other services are added in the Main class
        services.AddKeyedKernel((sp, kernel) =>
        {
            return new ChatDemo01(kernel);
        });

    }
}
