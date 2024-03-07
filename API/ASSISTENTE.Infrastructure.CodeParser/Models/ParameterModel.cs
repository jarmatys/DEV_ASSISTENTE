namespace ASSISTENTE.Infrastructure.CodeParser.Models;

internal sealed class ParameterModel
{
    private ParameterModel(string name, string? type, List<ModifierModel> modifiers)
    {
        Name = name;
        Type = type;
        Modifiers = modifiers;
    }
    
    public string Name { get; set; }
    public string? Type { get; set; }
    public List<ModifierModel> Modifiers { get; set; }
    
    public static ParameterModel Create(string name, string? type, List<ModifierModel> modifiers)
    {
        return new ParameterModel(name, type, modifiers);
    }
}