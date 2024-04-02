using ASSISTENTE.Domain.Commons;
using ASSISTENTE.Domain.Entities.Questions;

namespace ASSISTENTE.Domain.Entities.Answers;

public sealed class Answer : AuditableEntity
{
    private Answer(string text, string prompt, string client, string model, int promptTokens, int completionTokens)
    {
        Text = text;
        Prompt = prompt;
        Client = client;
        Model = model;
        PromptTokens = promptTokens;
        CompletionTokens = completionTokens;
        
        Question = null!;
    }
    
    public string Text { get; private set; }
    public string Prompt { get; private set; }   
    public string Client { get; private set; }
    public string Model { get; private set; }
    public int PromptTokens { get; private set; }
    public int CompletionTokens { get; private set; }
    
    # region NAVIGATION PROPERTIES
    
    public int QuestionId { get; private set; }
    public Question Question { get; private set; }
    
    # endregion
    
    public static Result<Answer> Create(
        string text, 
        string prompt, 
        string client, 
        string model, 
        int promptTokens, 
        int completionTokens)
    {
        return new Answer(text, prompt, client, model, promptTokens, completionTokens);
    }
}