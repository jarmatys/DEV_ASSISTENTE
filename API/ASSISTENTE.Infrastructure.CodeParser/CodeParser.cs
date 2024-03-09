using CSharpFunctionalExtensions;
using ASSISTENTE.Infrastructure.CodeParser.Errors;
using ASSISTENTE.Infrastructure.CodeParser.Extensions;
using ASSISTENTE.Infrastructure.CodeParser.Models;
using ASSISTENTE.Infrastructure.CodeParser.ValueObjects;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ASSISTENTE.Infrastructure.CodeParser;

internal sealed class CodeParser : ICodeParser
{
    public Result<CodeContent> Parse(CodeFile file)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(file.Content);

        if (syntaxTree.GetRoot() is not CompilationUnitSyntax root)
            return Result.Failure<CodeContent>(CodeParserErrors.FailedToParseCodeContent.Build());

        var namespaces = root.GetNamespaces();

        var classes = (
            from classDeclaration in root.GetClasses()
            let className = classDeclaration.GetName()
            let modifiers = classDeclaration.GetModifiers()
            let parameters = classDeclaration.GetParameters()
            let properties = classDeclaration.GetProperties()
            let methods = classDeclaration.GetMethods()
            select ClassModel.Create(className, file.FileName, modifiers, namespaces, properties, methods)
        ).ToList();

        return CodeContent.Create(classes);
    }
}