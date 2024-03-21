using CSharpFunctionalExtensions;using ASSISTENTE.Infrastructure.CodeParser.ValueObjects;

namespace ASSISTENTE.Infrastructure.CodeParser;

public interface ICodeParser
{
    public Result<CodeContent> Parse(CodePath codePath);
}