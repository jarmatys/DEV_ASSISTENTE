namespace ASSISTENTE.Infrastructure.LLM.Ollama.Settings;

public sealed class OllamaSettings
{
    public required string Url { get; init; }
    public required string SelectedModel  { get; init; }
}