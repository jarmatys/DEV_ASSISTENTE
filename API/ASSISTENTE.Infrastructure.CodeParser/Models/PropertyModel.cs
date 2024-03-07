namespace ASSISTENTE.Infrastructure.CodeParser.Models;

internal sealed class PropertyModel
{
    private PropertyModel(
        string name, 
        string? type, 
        List<string>? accessors, 
        List<ModifierModel>? modifiers)
    {
        Name = name;
        Type = type;
        Accessors = accessors;
        Modifiers = modifiers;
    }
    
    public string Name { get; }
    public string? Type { get; }
    public List<string>? Accessors { get; }
    public List<ModifierModel>? Modifiers { get; }
    
    public static PropertyModel Create(
        string name, 
        string? type, 
        List<string>? accessors, 
        List<ModifierModel>? modifiers)
    {
        return new PropertyModel(name, type, accessors, modifiers);
    }
}