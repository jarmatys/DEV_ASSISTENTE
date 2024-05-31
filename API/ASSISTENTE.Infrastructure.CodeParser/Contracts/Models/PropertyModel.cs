using System.Diagnostics;

namespace ASSISTENTE.Infrastructure.CodeParser.Contracts.Models;

[DebuggerDisplay("{Type} {Name}")]
public sealed class PropertyModel
{
    private PropertyModel(
        string name, 
        string? type, 
        IEnumerable<string>? accessors, 
        List<ModifierModel>? modifiers)
    {
        Name = name;
        Type = type;
        Accessors = accessors;
        Modifiers = modifiers;
    }

    private string Name { get; }
    private string? Type { get; }
    private IEnumerable<string>? Accessors { get; }
    private List<ModifierModel>? Modifiers { get; }
    
    public static PropertyModel Create(
        string name, 
        string? type, 
        IEnumerable<string>? accessors, 
        List<ModifierModel>? modifiers)
    {
        return new PropertyModel(name, type, accessors, modifiers);
    }
    
    public override string ToString()
    {
        var modifiers = Modifiers != null ? string.Join(" ", Modifiers) : string.Empty;
        var accessors = Accessors != null ? "{ " + string.Join("; ", Accessors) + " }" : string.Empty;
        
        return $"{modifiers} {Type} {Name} {accessors}";
    }
}