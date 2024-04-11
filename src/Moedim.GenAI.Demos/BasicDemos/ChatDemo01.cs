using Microsoft.SemanticKernel;

using Moedim.GenAI.Demos.Abstractions;

namespace Moedim.GenAI.Demos.BasicDemos;

/// <summary>
/// Represents a basic demo of getting prompt executed part of the kernel.
/// </summary>
public class ChatDemo01(Kernel kernel) : BaseDemo(kernel)
{
    /// <summary>
    /// Gets the name of the basic demo.
    /// </summary>
    public override string Name => nameof(ChatDemo01);

    /// <summary>
    /// Handles the user prompt and returns the response.
    /// </summary>
    /// <param name="kernel">The kernel instance.</param>
    /// <param name="userPrompt">The user prompt.</param>
    /// <returns>The response to the user prompt.</returns>
    protected override async Task<string?> HandlePrompt(Kernel kernel, string userPrompt)
    {
        return await kernel.InvokePromptAsync<string>(userPrompt);
    }
}
