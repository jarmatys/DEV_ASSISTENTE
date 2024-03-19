using CSharpFunctionalExtensions;
using ASSISTENTE.Infrastructure.CodeParser.Models;

namespace ASSISTENTE.Infrastructure.CodeParser.ValueObjects;

public sealed class CodeContent
{
    public IEnumerable<string> Classes { get; }

    private CodeContent(IEnumerable<string> classes)
    {
        Classes = classes;
    }
    
    public static Result<CodeContent> Create(IEnumerable<ClassModel> classes)
    {
        var classesContent = classes.Select(classModel => classModel.ToString()).ToList();
        
        return new CodeContent(classesContent);
    }
}