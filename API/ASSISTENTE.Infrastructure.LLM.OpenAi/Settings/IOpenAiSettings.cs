namespace ASSISTENTE.Infrastructure.LLM.OpenAi.Settings;

public interface IOpenAiSettings
{
    OpenAiSettings OpenAi { get; init; }
}