
using CommandLine;

public class CliOptions
{
    [Option('t', "type", Required = true, HelpText = $"Type of the job can be {nameof(CommandLine)}")]
    public string? Type { get; set; }
}