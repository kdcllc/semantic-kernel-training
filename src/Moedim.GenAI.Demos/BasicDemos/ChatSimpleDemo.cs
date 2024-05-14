using Microsoft.SemanticKernel;

using Moedim.GenAI.Demos.Abstractions;

namespace Moedim.GenAI.Demos.BasicDemos;

/// <summary>
/// Represents a basic demo of getting prompt executed part of the kernel.
/// </summary>
public class ChatSimpleDemo(Kernel kernel) : BaseDemo(kernel)
{
    /// <summary>
    /// Gets the name of the basic demo.
    /// </summary>
    public override string Name => nameof(ChatSimpleDemo);

    /// <summary>
    /// Handles the user prompt and returns the response.
    /// </summary>
    /// <param name="kernel">The kernel instance.</param>
    /// <param name="userPrompt">The user prompt.</param>
    /// <returns>The response to the user prompt.</returns>
    protected override async Task<string?> HandlePromptAsync(Kernel kernel, string userPrompt, CancellationToken cancellationToken )
    {
        KernelArguments arguments = [];
        var serviceId = nameof(ChatSimpleDemo);
        arguments.ExecutionSettings = new Dictionary<string, PromptExecutionSettings>()
        {
            { serviceId, new PromptExecutionSettings() }
        };

        return await kernel.InvokePromptAsync<string>(userPrompt, arguments, cancellationToken: cancellationToken);
    }
}
