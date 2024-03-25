using ASSISTENTE.Infrastructure.CodeParser.Errors;
using CSharpFunctionalExtensions;
using ASSISTENTE.Infrastructure.CodeParser.Models;

namespace ASSISTENTE.Infrastructure.CodeParser.ValueObjects;

public sealed class CodeContent
{
    public string Title { get; }
    public IEnumerable<string> CodeBlocks { get; }

    private CodeContent(string title, IEnumerable<string> codeBlocks)
    {
        Title = title;
        CodeBlocks = codeBlocks;
    }

    public static Result<CodeContent> Create(string title, IEnumerable<ClassModel> classes)
    {
        var blocks = new List<string>();

        foreach (var classContent in classes)
        {
            var tableOfContents = classContent.TableOfContents();

            var blocksToAdd = classContent
                .GetMethods()
                .Select(method => CreateMethodBlock(title, method, tableOfContents))
                .ToList();

            if (blocksToAdd.Count != 0)
            {
                blocks.AddRange(blocksToAdd);
            }
            else
            {
                blocks.Add(CreatePropertiesBlock(title, tableOfContents));
            }
        }
        
        return blocks.Count == 0
            ? Result.Failure<CodeContent>(CodeParserErrors.EmptyContent.Build(title))
            : new CodeContent(title, blocks);
    }

    private static string CreateMethodBlock(string title, string method, string tableOfContents)
    {
        return $"# File name: {title}\n\n{tableOfContents}\n\nMethod implementation: \n{method}";
    }

    private static string CreatePropertiesBlock(string title, string tableOfContents)
    {
        return $"# File name: {title}\n\n{tableOfContents}";
    }
}