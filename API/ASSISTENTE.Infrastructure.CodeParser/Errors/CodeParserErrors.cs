using ASSISTENTE.Common.Results;

namespace ASSISTENTE.Infrastructure.CodeParser.Errors;

internal static class CodeParserErrors
{
    public static readonly Error FailedToParseCodeContent = new(
        "CodeParser.FailedToParseCodeContent", "Failed to parse code content");
    
    public static readonly Error EmptyContent = new(
        "CodeParser.EmptyContent", "File content is empty");
}