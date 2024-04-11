
using CommandLine;

public class CliOptions
{
    [Option('t', "type", Required = false, HelpText = $"Name of the demo {nameof(CommandLine)}")]
    public string? Type { get; set; }
}