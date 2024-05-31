using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.CodeParser.Contracts;

public interface ICodeParser
{
    public Result<CodeContent> Parse(CodePath codePath);
}