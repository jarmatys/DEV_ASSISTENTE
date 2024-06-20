namespace ASSISTENTE.Infrastructure.LLM.Settings;

public interface ILlmSettings
{
    LlmSettings Llm { get; init; }
}