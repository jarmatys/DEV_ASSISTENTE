using ASSISTENTE.Common.Results;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.Embeddings.ValueObjects;

public sealed class EmbeddingText : ValueObject
{
    public string Text { get; }

    private EmbeddingText(string text)
    {
        Text = text;
    }
    
    public static Result<EmbeddingText> Create(string text)
    {
        if (string.IsNullOrEmpty(text))
            return Result.Failure<EmbeddingText>(EmbeddingTextErrors.EmptyContent.Build());
        
        return new EmbeddingText(text);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Text;
    }
}

public static class EmbeddingTextErrors
{
    public static readonly Error EmptyContent = new(
        "EmbeddingContent.EmptyContent", "Text cannot be empty.");
}