using Moedim.GenAI.Demos.Abstractions.Options;
using Moedim.GenAI.Demos.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel;

namespace Microsoft.Extensions.DependencyInjection;

public static class DemosAbstractionsServiceCollectionExtensions
{
    public static IServiceCollection AddDemosKernel(this IServiceCollection services)
    {
        // add options for OpenAIOptions with validation
        services.AddOptions<OpenAIOptions>()
                .ValidateOnStart()
                .ValidateDataAnnotations()
                .Configure<IConfiguration>((o, c) =>
                {
                    c.GetSection(nameof(OpenAIOptions)).Bind(o);
                });

        
        // add chat completion service seperately
        services.AddSingleton<IChatCompletionService>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<OpenAIOptions>>().Value;

            if (options.Azure)
            {
                // this is where we can change from api key to managed identity
                return new AzureOpenAIChatCompletionService(options.CompletionModelId, options.Endpoint, options.Key);
            }

            return new OpenAIChatCompletionService(options.CompletionModelId, options.Key);
        });

        services.AddKeyedTransient<Kernel>("DemosnKernel", (sp, key) =>
        {
            // Create a collection of plugins that the kernel will use
            KernelPluginCollection pluginCollection = new();

            return new Kernel(sp, pluginCollection);
        });

        // add basic demo
        services.AddTransient<IDemo, BasicDemo>();

        return services;
    }
}
