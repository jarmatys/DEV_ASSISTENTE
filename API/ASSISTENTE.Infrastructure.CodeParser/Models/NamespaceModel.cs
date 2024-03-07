namespace ASSISTENTE.Infrastructure.CodeParser.Models;

internal sealed class NamespaceModel
{
    private NamespaceModel(string name)
    {
        Name = name;
    }
    
    public string Name { get; }
    
    public static NamespaceModel Create(string name)
    {
        return new NamespaceModel(name);
    }
}