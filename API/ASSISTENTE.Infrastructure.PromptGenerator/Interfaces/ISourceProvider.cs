namespace ASSISTENTE.Infrastructure.PromptGenerator.Interfaces
{
    public interface ISourceProvider
    {
        public string Prompt<T>(string question) where T : struct, Enum;
    }
}