namespace ASSISTENTE.Infrastructure.PromptGenerator
{
    public interface ISourceProvider
    {
        public string Prompt<T>(string question) where T : struct, Enum;
    }
}