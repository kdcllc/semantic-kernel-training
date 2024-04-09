using Dumpify;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

using Moedim.GenAI.Demos.Abstractions.Options;

using System.Drawing;

namespace Moedim.GenAI.Demos.Abstractions;

/// <summary>
/// Represents a base class for demos.
/// </summary>
public abstract class BaseDemo : IDemo
{
    private Kernel? _kernel;
    private ChatHistory _history = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseDemo"/> class.
    /// </summary>
    /// <param name="options">The OpenAI options.</param>
    /// <param name="loggerFactory">The logger factory.</param>
    public BaseDemo([FromKeyedServices("DemosnKernel")] Kernel kernel)
    {
        _kernel = kernel;
    }

    /// <summary>
    /// Gets the name of the demo.
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// Runs the demo asynchronously.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        _history.AddSystemMessage(@"You're a virtual assistant that helps people find information.");

        while (!cancellationToken.IsCancellationRequested)
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
                _history.AddAssistantMessage(result);
                result.Dump();
            }
        }
    }

    /// <summary>
    /// Gets the screen prompt for the demo.
    /// </summary>
    public virtual string ScreenPrompt => "How can I help you?";

    /// <summary>
    /// Handles the user prompt and returns the response.
    /// </summary>
    /// <param name="kernel">The kernel instance.</param>
    /// <param name="userPrompt">The user prompt.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation. The task result contains the response.</returns>
    protected abstract Task<string?> HandlePrompt(Kernel kernel, string userPrompt);
}
