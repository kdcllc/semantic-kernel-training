
using CommandLine;

/// <summary>
/// Represents the command-line options for the application.
/// </summary>
public class CliOptions
{
    /// <summary>
    /// Gets or sets the name of the demo <see cref="CommandLine"/>.
    /// </summary>
    [Option('t', "type", Required = false, HelpText = $"Name of the demo {nameof(CommandLine)}")]
    public string? Type { get; set; }
}