using CSharpFunctionalExtensions;
using SOFTURE.Results;

namespace ASSISTENTE.Infrastructure.Vision.Contracts;

public sealed class Recognition : ValueObject
{
    private Recognition(string text)
    {
        Text = text;
    }

    public string Text { get; }

    public static Result<Recognition> Create(string? text)
    {
        if (string.IsNullOrEmpty(text))
            return Result.Failure<Recognition>(CommonErrors.EmptyParameter.Build());

        return new Recognition(text);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Text;
    }
}