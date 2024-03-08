using System.Diagnostics;

namespace ASSISTENTE.Infrastructure.CodeParser.Models;

[DebuggerDisplay("{Type} {Name}")]
public sealed class ParameterModel
{
    private ParameterModel(string name, string? type, List<ModifierModel> modifiers)
    {
        Name = name;
        Type = type;
        Modifiers = modifiers;
    }

    private string Name { get; }
    private string? Type { get; }
    private List<ModifierModel> Modifiers { get; }
    
    public static ParameterModel Create(string name, string? type, List<ModifierModel> modifiers)
    {
        return new ParameterModel(name, type, modifiers);
    }
    
    public override string ToString()
    {
        var modifiers = string.Join(" ", Modifiers);

        return $"{modifiers} {Type} {Name}";
    }
}