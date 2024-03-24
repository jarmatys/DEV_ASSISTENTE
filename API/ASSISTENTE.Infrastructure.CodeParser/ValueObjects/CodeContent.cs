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
            // TODO: Split content by methods 
            // TODO: Based on method names in whole Class create "table of contents"
            
            // blocks.Add(classContent.ToString());
        }
        
        return new CodeContent(title, blocks);
    }
}