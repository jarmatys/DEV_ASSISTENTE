using ASSISTENTE.Infrastructure.CodeParser;
using ASSISTENTE.Infrastructure.CodeParser.ValueObjects;
using ASSISTENTE.Infrastructure.Errors;
using ASSISTENTE.Infrastructure.Interfaces;
using ASSISTENTE.Infrastructure.MarkDownParser;
using ASSISTENTE.Infrastructure.MarkDownParser.ValueObjects;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.Services;

public sealed class FileParser(
    IMarkDownParser markDownParser,
    ICodeParser codeParser) : IFileParser
{
    public Result<List<string>> Parse(string filePath)
    {
        if (filePath.EndsWith(".md"))
        {
            return FilePath.Create(filePath)
                .Bind(markDownParser.Parse)
                .Map(fileContent => new List<string> { fileContent.Content });
        }

        if (filePath.EndsWith(".cs"))
        {
            return CodeFile.Create(filePath)
                .Bind(codeParser.Parse)
                .Map(fileContent => fileContent.Classes);
        }

        return Result.Failure<List<string>>(FileParserErrors.UnsupportedFormat.Build());
    }
}