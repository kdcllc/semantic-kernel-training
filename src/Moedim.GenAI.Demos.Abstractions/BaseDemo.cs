﻿using Dumpify;

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

using System.Drawing;

namespace Moedim.GenAI.Demos.Abstractions;

/// <summary>
/// Represents a base class for demos.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="BaseDemo"/> class.
/// </remarks>
/// <param name="options">The OpenAI options.</param>
/// <param name="loggerFactory">The logger factory.</param>
public abstract class BaseDemo(Kernel kernel) : IDemo
{
    private Kernel? _kernel = kernel;

    protected ChatHistory _history = [];

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
        _history.AddSystemMessage(SystemMessage);

        SystemMessage.Dump(colors: new ColorConfig { PropertyValueColor = Color.YellowGreen} );

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

            _history.AddUserMessage(query);

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
    public virtual string ScreenPrompt => "How can I help you? (if done type 'exit')";

    /// <summary>
    /// Gets the system message for the demo.
    /// </summary>
    public virtual string SystemMessage => "You're a virtual assistant that helps people find information.";

    /// <summary>
    /// Handles the user prompt and returns the response.
    /// </summary>
    /// <param name="kernel">The kernel instance.</param>
    /// <param name="userPrompt">The user prompt.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation. The task result contains the response.</returns>
    protected abstract Task<string?> HandlePrompt(Kernel kernel, string userPrompt);
}
