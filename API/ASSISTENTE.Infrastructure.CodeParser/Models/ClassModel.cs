using System.Diagnostics;

namespace ASSISTENTE.Infrastructure.CodeParser.Models;

[DebuggerDisplay("{Name}")]
public sealed class ClassModel
{
    private ClassModel( 
        string name, 
        string fileName,
        IEnumerable<ModifierModel> modifiers, 
        IEnumerable<NamespaceModel> namespaces, 
        IEnumerable<PropertyModel> properties, 
        IEnumerable<MethodModel> methods)
    {
        Name = name;
        FileName = fileName;
        Modifiers = modifiers;
        Namespaces = namespaces;
        Properties = properties;
        Methods = methods;
    }
    
    private string Name { get; }
    private string FileName { get; }
    private IEnumerable<ModifierModel> Modifiers { get; }
    private IEnumerable<NamespaceModel> Namespaces { get; }
    private IEnumerable<PropertyModel> Properties { get; }
    private IEnumerable<MethodModel> Methods { get; }
    
    public static ClassModel Create(
        string name, 
        string fileName,
        IEnumerable<ModifierModel> modifiers, 
        IEnumerable<NamespaceModel> namespaces, 
        IEnumerable<PropertyModel> properties, 
        IEnumerable<MethodModel> methods)
    {
        return new ClassModel(name, fileName, modifiers, namespaces, properties, methods);
    }
    
    public override string ToString()
    {
        var modifiers = string.Join(" ", Modifiers);
        var namespaces = string.Join("\n", Namespaces);
        var properties = string.Join("\n", Properties);
        var methods = string.Join("\n", Methods);
        
        return $"{namespaces}\n\n\n{modifiers} class {Name}" + "\n{" + $"\n{properties}\n{methods}" + "\n}";
    }
    
    public string GetFileName()
    {
        return FileName;
    }
}