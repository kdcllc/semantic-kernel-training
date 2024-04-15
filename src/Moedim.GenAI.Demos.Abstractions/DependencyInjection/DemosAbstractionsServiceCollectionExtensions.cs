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
        string? name = null) where TDemo : class, IDemo
    {
        // default keyed value
        var keyed = typeof(TDemo).Name;

        if (!string.IsNullOrWhiteSpace(name))
        {
            keyed = name;
        }

        services.AddOptions<OpenAIOptions>(keyed)
            .ValidateOnStart()
            .ValidateDataAnnotations()
            .Configure<IConfiguration>((o, c) =>
            {
                c.GetSection(nameof(OpenAIOptions)).Bind(o);
            });

        services.TryAddKeyedSingleton<Kernel>(
            keyed, 
            (sp, key) =>
            {
                var options = sp.GetRequiredService<IOptionsMonitor<OpenAIOptions>>().Get(keyed);

                var kernel = Kernel.CreateBuilder();

                if (options.Azure)
                {
                    kernel.AddAzureOpenAIChatCompletion(
                        options.CompletionModelId, 
                        options.Endpoint, 
                        options.Key,
                        serviceId: keyed);
                }
                else
                {
                    kernel.AddOpenAIChatCompletion(
                        options.CompletionModelId, 
                        options.Key,
                        serviceId: keyed);
                }
  
                return kernel.Build();
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
