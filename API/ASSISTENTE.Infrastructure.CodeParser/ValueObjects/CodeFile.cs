using ASSISTENTE.Common.Results;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.CodeParser.ValueObjects;

public sealed class CodeFile
{
    private string Path { get; }
    
    public string Extension { get; }
    public string FileName { get; }
    public string Content { get; }

    private CodeFile(string path, string extension, string fileName, string content)
    {
        Path = path;
        Extension = extension;
        FileName = fileName;
        Content = content;
    }

    public static Result<CodeFile> Create(string path)
    {
        var extension = System.IO.Path.GetExtension(path);

        if (extension == string.Empty)
            return Result.Failure<CodeFile>(FilePathErrors.NotFound.Build());

        if (extension != ".cs")
            return Result.Failure<CodeFile>(FilePathErrors.InvalidFileExtension.Build());

        var fileName = System.IO.Path.GetFileName(path);
        var content = System.IO.File.ReadAllText(path);
        
        return new CodeFile(path, extension, fileName, content);
    }

    public static implicit operator string(CodeFile codeFile) => codeFile.Path;
}

public static class FilePathErrors
{
    public static readonly Error InvalidFileExtension = new(
        "CodePath.InvalidFileExtension", "Invalid file extension - only '.cs' files are allowed");

    public static readonly Error NotFound = new(
        "CodePath.NotFound", "File not found");
}