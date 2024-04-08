using Moedim.GenAI.Demos.Abstractions.Options;
using Moedim.GenAI.Demos.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConsoleServiceCollectionExtensions
{
    public static void ConfigureServices(HostBuilderContext hostBuilder, IServiceCollection services)
    {
        services.AddScoped<IMain, Main>();

        // add options for OpenAIOptions with validation
        services.AddOptions<OpenAIOptions>()
                .Bind(hostBuilder.Configuration.GetSection(nameof(OpenAIOptions)))
                .ValidateDataAnnotations();

        services.AddScoped<IDemo, BasicDemo>();
    }
}
