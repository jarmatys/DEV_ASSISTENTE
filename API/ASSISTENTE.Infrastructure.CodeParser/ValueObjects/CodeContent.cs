using CSharpFunctionalExtensions;
using ASSISTENTE.Infrastructure.CodeParser.Models;

namespace ASSISTENTE.Infrastructure.CodeParser.ValueObjects;

public sealed class CodeContent
{
    public string Title { get; set; }
    public IEnumerable<string> Classes { get; }

    private CodeContent(string title, IEnumerable<string> classes)
    {
        Title = title;
        Classes = classes;
    }
    
    public static Result<CodeContent> Create(string title, IEnumerable<ClassModel> classes)
    {
        var classesContent = classes.Select(classModel => classModel.ToString()).ToList();
        
        return new CodeContent(title, classesContent);
    }
}