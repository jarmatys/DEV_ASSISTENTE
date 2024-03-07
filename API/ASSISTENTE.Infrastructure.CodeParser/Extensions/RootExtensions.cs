using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ASSISTENTE.Infrastructure.CodeParser.Extensions;

internal static class RootExtensions
{
    public static IEnumerable<ClassDeclarationSyntax> GetClasses(this CompilationUnitSyntax root)
        => root.DescendantNodes().OfType<ClassDeclarationSyntax>();
}