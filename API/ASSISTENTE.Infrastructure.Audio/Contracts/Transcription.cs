using CSharpFunctionalExtensions;
using SOFTURE.Results;

namespace ASSISTENTE.Infrastructure.Audio.Contracts;

public sealed class Transcription : ValueObject
{
    private Transcription(string text)
    {
        Text = text;
    }

    public string Text { get; }

    public static Result<Transcription> Create(string? text)
    {
        if (string.IsNullOrEmpty(text))
            return Result.Failure<Transcription>(CommonErrors.EmptyParameter.Build());

        return new Transcription(text);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Text;
    }
}