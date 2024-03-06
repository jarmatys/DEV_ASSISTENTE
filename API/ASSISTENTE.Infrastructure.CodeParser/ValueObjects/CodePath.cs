using ASSISTENTE.Common;
using ASSISTENTE.Common.Extensions;

namespace ASSISTENTE.Infrastructure.CodeParser.ValueObjects;

public sealed class CodePath
{
    public string Path { get; }
    public string Extension { get; }

    private CodePath(string path, string extension)
    {
        Path = path;
        Extension = extension;
    }

    public static Result<CodePath> Create(string path)
    {
        var extension = System.IO.Path.GetExtension(path);

        if (extension == string.Empty)
            return Result<CodePath>.Fail(FilePathErrors.NotFound);

        if (extension != ".cs")
            return Result<CodePath>.Fail(FilePathErrors.InvalidFileExtension);

        return new CodePath(path, extension).ToResult();
    }

    public static implicit operator string(CodePath codePath) => codePath.Path;
}

public static class FilePathErrors
{
    public static readonly Error InvalidFileExtension = new(
        "CodePath.InvalidFileExtension", "Invalid file extension - only '.cs' files are allowed");

    public static readonly Error NotFound = new(
        "CodePath.NotFound", "File not found");
}