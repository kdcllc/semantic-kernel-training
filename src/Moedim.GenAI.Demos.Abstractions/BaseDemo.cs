using Microsoft.SemanticKernel;
using Dumpify;
using Moedim.GenAI.Demos.Abstractions.Options;
using Microsoft.Extensions.Options;
using System.Drawing;

namespace Moedim.GenAI.Demos.Abstractions;

public abstract class BaseDemo : IDemo
{
    private Kernel? _kernel;

    public BaseDemo(IOptions<OpenAIOptions> options)
    {
        _kernel = CreateKernel(options.Value);
    }

    public abstract string Name { get; }

    public virtual Kernel CreateKernel(OpenAIOptions options)
    {
        var kernel = Kernel.CreateBuilder();

        if (options.Azure){
            kernel.AddAzureOpenAIChatCompletion(options.CompletionModelId, options.Endpoint, options.Key);
        }
        else{
            kernel.AddOpenAIChatCompletion(options.CompletionModelId, options.Key);
        }

        return kernel.Build();
    }

    public async Task RunAsync()
    {
        while (true)
        {
            ScreenPrompt.Dump(colors: new ColorConfig { PropertyValueColor = Color.Aqua });

            var query = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(query))
            {
                continue;
            }

            // if exit then break
            if (query.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }
            
            var result = await HandlePrompt(_kernel!, query);

            if (result is null)
            {
                Console.WriteLine();
            }
            else
            {
                result.Dump();
            }
        }
    }

   public virtual string ScreenPrompt => "How can I help you?";

    protected abstract Task<string?> HandlePrompt(Kernel kernel, string userPrompt);

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
