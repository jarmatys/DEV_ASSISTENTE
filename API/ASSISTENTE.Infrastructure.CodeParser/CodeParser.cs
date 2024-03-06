using ASSISTENTE.Common;
using ASSISTENTE.Common.Extensions;
using ASSISTENTE.Infrastructure.CodeParser.Errors;
using ASSISTENTE.Infrastructure.CodeParser.ValueObjects;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ASSISTENTE.Infrastructure.CodeParser;

internal sealed class CodeParser : ICodeParser
{
    public Result<CodeContent> Parse(CodePath codePath)
    {
        var content = File.ReadAllText(codePath.Path);
        
        var syntaxTree = CSharpSyntaxTree.ParseText(content);

        if (syntaxTree.GetRoot() is not CompilationUnitSyntax root)
            return Result<CodeContent>.Fail(CodeParserErrors.FailedToParseCodeContent);
        
        var namespaces = root.DescendantNodes().OfType<UsingDirectiveSyntax>();
        var namespaceNames = namespaces.Select(x => x.Name?.ToString()).ToList();

        Console.WriteLine($"Namespaces: {namespaceNames}");
        
        var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
        
        foreach(var classDeclaration in classes)
        {
            var className = classDeclaration.Identifier.Text;
            Console.WriteLine($"Class: {className}");

            var classModifiers = classDeclaration.Modifiers.Select(x => x.Text).ToList();
            
            var parameters = classDeclaration.DescendantNodes().OfType<ParameterSyntax>();
            var parametersName = parameters.Select(x => x.Identifier.Text).ToList();

            Console.WriteLine($"Class: {className} - {classModifiers} - {parametersName}");
            
            var methods = classDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>();
            foreach (var method in methods)
            {
                var methodName = method.Identifier.Text;
                Console.WriteLine($"Method: {methodName}");

                var methodModifiers = method.Modifiers.Select(x => x.Text).ToList();

                var methodParameters = method.DescendantNodes().OfType<ParameterSyntax>().Where(x => x.Identifier.Text != "x");
                foreach (var parameter in methodParameters)
                {
                    var parameterName = parameter.Identifier.Text;
                    var parameterType = parameter.Type?.ToString();
                    var parameterModifiers = parameter.Modifiers.Select(x => x.Text).ToList();
                    
                    Console.WriteLine($"Parameter: {parameterName} - {parameterType} - {parameterModifiers}");
                }

                var methodReturnType = method.ReturnType?.ToString();
                
                Console.WriteLine($"Method: {methodName} - {methodReturnType} - {methodModifiers}");
            }

            var properties = classDeclaration.DescendantNodes().OfType<PropertyDeclarationSyntax>();
            foreach (var property in properties)
            {
                var propertyName = property.Identifier.Text;
                var propertyModifiers = property.Modifiers.Select(x => x.Text).ToList();
                var propertyType = property.Type?.ToString();
                var propertyAccessors = property.AccessorList?.Accessors.Select(x => x.Keyword.Text).ToList();
                
                Console.WriteLine($"Property: {propertyName} - {propertyType} - {propertyAccessors} - {propertyModifiers}");
            }
        }
        
        return CodeContent.Create(content)
            .OnFailure(error => Result<CodeContent>.Fail(error));
    }
}