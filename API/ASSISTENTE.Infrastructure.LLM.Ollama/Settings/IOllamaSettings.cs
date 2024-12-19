namespace ASSISTENTE.Infrastructure.LLM.Ollama.Settings;

public interface IOllamaSettings
{
    OllamaSettings Ollama { get; init; }
}