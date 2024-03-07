using ASSISTENTE.Common;

namespace ASSISTENTE.Infrastructure.CodeParser.Models;

internal sealed class ClassModel
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
    
    public string Name { get; }
    public string FileName { get; }
    public IEnumerable<ModifierModel> Modifiers { get; }
    public IEnumerable<NamespaceModel> Namespaces { get; }
    public IEnumerable<PropertyModel> Properties { get; }
    public IEnumerable<MethodModel> Methods { get; }
    
    
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
}