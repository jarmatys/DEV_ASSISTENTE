using ASSISTENTE.Common.Results;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.CodeParser.ValueObjects;

public sealed class CodePath
{
    private string Path { get; }
    
    public string Extension { get; }
    public string FileName { get; }
    public string Content { get; }

    private CodePath(string path, string extension, string fileName, string content)
    {
        Path = path;
        Extension = extension;
        FileName = fileName;
        Content = content;
    }

    public static Result<CodePath> Create(string path)
    {
        var extension = System.IO.Path.GetExtension(path);

        if (extension == string.Empty)
            return Result.Failure<CodePath>(FilePathErrors.NotFound.Build());

        if (extension != ".cs")
            return Result.Failure<CodePath>(FilePathErrors.InvalidFileExtension.Build());

        var fileName = System.IO.Path.GetFileName(path);
        var content = File.ReadAllText(path);
        
        return new CodePath(path, extension, fileName, content);
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