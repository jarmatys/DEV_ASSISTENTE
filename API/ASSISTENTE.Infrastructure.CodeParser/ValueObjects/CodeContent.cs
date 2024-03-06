using ASSISTENTE.Common;
using ASSISTENTE.Common.Extensions;

namespace ASSISTENTE.Infrastructure.CodeParser.ValueObjects;

public sealed class CodeContent
{
    public string Content { get; }

    private CodeContent(string content)
    {
        Content = content;
    }
    
    public static Result<CodeContent> Create(string content)
    {
        return new CodeContent(content).ToResult();
    }
}