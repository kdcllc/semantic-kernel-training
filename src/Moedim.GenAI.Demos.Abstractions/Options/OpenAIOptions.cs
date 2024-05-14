namespace Moedim.GenAI.Demos.Abstractions.Options;

public class OpenAIOptions
{
    public bool Azure { get; set; } = true;

    public string Key { get; set; } = string.Empty;

    public string CompletionModelId { get; set; } = string.Empty;

    public string Endpoint { get; set; } = string.Empty;
}
