namespace ASSISTENTE.Infrastructure.PromptGenerator.Contracts
{
    public interface IContextProvider
    {
        public string Prompt<T>(string question) where T : struct, Enum;
    }
}