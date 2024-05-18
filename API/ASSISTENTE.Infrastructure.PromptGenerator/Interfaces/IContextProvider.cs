namespace ASSISTENTE.Infrastructure.PromptGenerator.Interfaces
{
    public interface IContextProvider
    {
        public string Prompt<T>(string question) where T : struct, Enum;
    }
}