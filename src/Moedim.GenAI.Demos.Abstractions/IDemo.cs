﻿namespace Moedim.GenAI.Demos.Abstractions;

public interface IDemo
{
    public string Name { get; }

    /// <summary>
    /// Runs the demo.
    /// </summary>
    /// <returns>A task representing the asynchronous running operation.</returns>
    Task RunAsync(CancellationToken cancellationToken);
}