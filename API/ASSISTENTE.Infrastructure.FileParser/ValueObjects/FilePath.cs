using ASSISTENTE.Common.Results;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.FileParser.ValueObjects;

public sealed class FilePath
{
    public string Path { get; }
    public string Extension { get; }

    private FilePath(string path, string extension)
    {
        Path = path;
        Extension = extension;
    }

    public static Result<FilePath> Create(string path)
    {
        var extension = System.IO.Path.GetExtension(path);

        if (extension == string.Empty)
            return Result.Failure<FilePath>(FilePathErrors.NotFound.Build());

        if (extension != ".md")
            return Result.Failure<FilePath>(FilePathErrors.InvalidFileExtension.Build());

        return new FilePath(path, extension);
    }

    public static implicit operator string(FilePath filePath) => filePath.Path;
}

public static class FilePathErrors
{
    public static readonly Error InvalidFileExtension = new(
        "FilePath.InvalidFileExtension", "Invalid file extension - only '.md' files are allowed");

    public static readonly Error NotFound = new(
        "FilePath.NotFound", "File not found");
}