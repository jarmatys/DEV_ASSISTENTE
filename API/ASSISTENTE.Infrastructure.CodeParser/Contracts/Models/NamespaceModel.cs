using System.Diagnostics;

namespace ASSISTENTE.Infrastructure.CodeParser.Contracts.Models;

[DebuggerDisplay("{Name}")]
public sealed class NamespaceModel
{
    private NamespaceModel(string name)
    {
        Name = name;
    }

    private string Name { get; }
    
    public static NamespaceModel Create(string name)
    {
        return new NamespaceModel(name);
    }
    
    public override string ToString()
    {
        return Name;
    }
}