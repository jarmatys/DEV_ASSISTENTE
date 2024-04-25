using ASSISTENTE.Infrastructure.CodeParser.ValueObjects;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.CodeParser;

public interface ICodeParser
{
    public Result<CodeContent> Parse(CodePath codePath);
}