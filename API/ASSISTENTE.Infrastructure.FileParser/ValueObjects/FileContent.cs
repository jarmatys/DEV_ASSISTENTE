using ASSISTENTE.Common;
using ASSISTENTE.Common.Extensions;

namespace ASSISTENTE.Infrastructure.FileParser.ValueObjects;

public sealed class FileContent
{
    public string Content { get; }

    private FileContent(string content)
    {
        Content = content;
    }
    
    public static Result<FileContent> Create(string content)
    {
        return new FileContent(content).ToResult();
    }
}