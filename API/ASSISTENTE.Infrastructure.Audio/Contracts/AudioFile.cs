using CSharpFunctionalExtensions;
using SOFTURE.Results;

namespace ASSISTENTE.Infrastructure.Audio.Contracts;

public sealed class AudioFile : ValueObject
{
    private AudioFile(string name, Stream stream)
    {
        Name = name;
        Stream = stream;
    }
    
    public string Name { get; }
    public Stream Stream { get; }
    
    public static Result<AudioFile> Create(string name, Stream stream)
    {
        if (string.IsNullOrEmpty(name))
            return Result.Failure<AudioFile>(EmbeddingTextErrors.EmptyContent.Build());
        
        return new AudioFile(name, stream);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Name;
    }
}

public static class EmbeddingTextErrors
{
    public static readonly Error EmptyContent = new(
        "Prompt.EmptyContent", "Prompt cannot be empty.");
}