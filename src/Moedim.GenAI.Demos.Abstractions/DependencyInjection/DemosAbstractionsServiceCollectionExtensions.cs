using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

using Moedim.GenAI.Demos.Abstractions;
using Moedim.GenAI.Demos.Abstractions.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class DemosAbstractionsServiceCollectionExtensions
{
    public static IServiceCollection AddKeyedKernel<TDemo>(
        this IServiceCollection services,
        Func<IServiceProvider, Kernel, TDemo> configureDemo,
        Action<IServiceProvider, KernelPluginCollection>? configuePlugins=null,
        string? name = null) where TDemo : class, IDemo
    {
        // default keyed value
        var keyed = typeof(TDemo).Name;

        if (!string.IsNullOrWhiteSpace(name))
        {
            keyed = name;
        }

        services.AddOptions<OpenAIOptions>("global")
            .ValidateOnStart()
            .ValidateDataAnnotations()
            .Configure<IConfiguration>((o, c) =>
            {
                c.GetSection(nameof(OpenAIOptions)).Bind(o);
            });

        // add chat completion service separately
        services.TryAddSingleton<IChatCompletionService>(
            sp =>
            {
                var options = sp.GetRequiredService<IOptionsMonitor<OpenAIOptions>>().Get("global");

                if (options.Azure)
                {
                    // this is where we can change from api key to managed identity
                    return new AzureOpenAIChatCompletionService(options.CompletionModelId, options.Endpoint, options.Key);
                }

                return new OpenAIChatCompletionService(options.CompletionModelId, options.Key);
            });


        services.TryAddKeyedSingleton<Kernel>(
            keyed, 
            (sp, key) =>
            {
                var kernel = Kernel.CreateBuilder();
                // Create a collection of plugins that the kernel will use
                KernelPluginCollection pluginCollection = new();

                if (configuePlugins != null)
                {
                    configuePlugins(sp, pluginCollection);
                }

                return new Kernel(sp, pluginCollection);
            });

        // add demo
        services.AddSingleton<IDemo>(sp =>{
            var kernel = sp.GetKeyedService<Kernel>(keyed);
            if (kernel == null)
            {
                throw new InvalidOperationException("Kernel not found");
            }
            return configureDemo(sp, kernel);
        });

        return services;
    }
}
