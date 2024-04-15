using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Plugins.Core;

using Moedim.GenAI.Demos.BasicDemos;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConsoleServiceCollectionExtensions
{
    public static void ConfigureServices(HostBuilderContext hostBuilder, IServiceCollection services)
    {
        services.AddScoped<IMain, Main>();

        // add the ChatSimpleDemo to the DI container
        services.AddKeyedKernel((sp, kernel) =>
        {
            return new ChatSimpleDemo(kernel);
        });


        // add build in plugin into the DI container
        // Microsoft.SemanticKernel.Plugins.Core (nuget package) alpha version
        #pragma warning disable SKEXP0050 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

        services.AddKeyedSingleton<TimePlugin>("time");

        // add the ChatTimePluginDemo to the DI container
        services.AddKeyedKernel(
            configureDemo:(sp, kernel) =>
            {
                // adds after the kernel is created
                KernelPlugin[] plugins = [kernel.CreatePluginFromPromptDirectory("../Moedim.GenAI.Demos/Plugins/Prompts/"),];
                kernel.Plugins.AddRange (plugins);

                return new ChatTimePluginDemo(kernel);
            },
            configurePlugins: (sp, col) =>
            {
                var plug = sp.GetRequiredKeyedService<TimePlugin>("time");
                col.AddFromObject(plug, "time");
            });
    }
}
