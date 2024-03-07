using ASSISTENTE.Infrastructure.CodeParser.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ASSISTENTE.Infrastructure.CodeParser.Extensions;

internal static class CodeExtensions
{
    public static IEnumerable<NamespaceModel> GetNamespaces(this CompilationUnitSyntax root)
    {
        var namespaces = root.DescendantNodes().OfType<UsingDirectiveSyntax>();
        
        return namespaces.Where(x => x.Name != null).Select(x => NamespaceModel.Create(x.Name!.ToString())).ToList();
    }

    public static string GetName(this ClassDeclarationSyntax classDeclaration)
    {
        return classDeclaration.Identifier.Text;
    }

    public static IEnumerable<ModifierModel> GetModifiers(this ClassDeclarationSyntax classDeclaration)
    {
        return classDeclaration.Modifiers.Select(x => ModifierModel.Create(x.Text)).ToList();
    }

    public static IEnumerable<ParameterModel> GetParameters(this ClassDeclarationSyntax classDeclaration)
    {
        var parameters = classDeclaration.DescendantNodes().OfType<ParameterSyntax>()
            .Where(x => x.Identifier.Text != "x");

        return (
            from parameter in parameters
            let parameterName = parameter.Identifier.Text
            let parameterType = parameter.Type?.ToString()
            let parameterModifiers = parameter.Modifiers.Select(x => ModifierModel.Create(x.Text)).ToList()
            select ParameterModel.Create(parameterName, parameterType, parameterModifiers)
        ).ToList();
    }

    public static IEnumerable<PropertyModel> GetProperties(this ClassDeclarationSyntax classDeclaration)
    {
        var properties = classDeclaration.DescendantNodes().OfType<PropertyDeclarationSyntax>();

        return (
            from property in properties
            let name = property.Identifier.Text
            let type = property.Type?.ToString()
            let accessors = property.AccessorList?.Accessors.Select(x => x.Keyword.Text).ToList()
            let modifiers = property.Modifiers.Select(x => ModifierModel.Create(x.Text)).ToList()
            select PropertyModel.Create(name, type, accessors, modifiers)
        ).ToList();
    }

    public static IEnumerable<ParameterModel> GetParameters(this MethodDeclarationSyntax classDeclaration)
    {
        var parameters = classDeclaration.DescendantNodes().OfType<ParameterSyntax>()
            .Where(x => x.Identifier.Text != "x");

        return (
            from parameter in parameters
            let parameterName = parameter.Identifier.Text
            let parameterType = parameter.Type?.ToString()
            let parameterModifiers = parameter.Modifiers.Select(x => ModifierModel.Create(x.Text)).ToList()
            select ParameterModel.Create(parameterName, parameterType, parameterModifiers)
        ).ToList();
    }

    public static IEnumerable<MethodModel> GetMethods(this ClassDeclarationSyntax classDeclaration)
    {
        var methods = classDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>();

        return (
            from method in methods
            let name = method.Identifier.Text
            let modifiers = method.Modifiers.Select(x => ModifierModel.Create(x.Text)).ToList()
            let body = method.Body?.ToString()
            let returnType = method.ReturnType?.ToString()
            let parameters = method.GetParameters()
            select MethodModel.Create(name, returnType, body, modifiers, parameters)
        ).ToList();
    }
}