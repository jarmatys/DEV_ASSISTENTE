using ASSISTENTE.Domain.Commons;

namespace ASSISTENTE.Domain.Entities.Questions;

public sealed class Question : AuditableEntity // TODO: Prepare repo and use them in RecallAsync method
{
    private Question(string text, IEnumerable<float> embeddings)
    {
        Text = text;
        Embeddings = embeddings;
    }
    
    public string Text { get; private set; }
    public IEnumerable<float> Embeddings { get; private set; }
    
    public static Result<Question> Create(string text, IEnumerable<float> embeddings)
    {
        return new Question(text, embeddings);
    }
}