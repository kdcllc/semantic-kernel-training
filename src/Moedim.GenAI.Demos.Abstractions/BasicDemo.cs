using Microsoft.SemanticKernel;
using Microsoft.Extensions.Options;
using Moedim.GenAI.Demos.Abstractions.Options;

namespace Moedim.GenAI.Demos.Abstractions;

public class BasicDemo : BaseDemo
{
    // implement 
    public BasicDemo(IOptions<OpenAIOptions> options) : base(options)
    {
    }

    public override string Name => "basic";

    protected override async Task<string?> HandlePrompt(Kernel kernel, string userPrompt)
    {
        return await kernel.InvokePromptAsync<string>(userPrompt);
    }
}
