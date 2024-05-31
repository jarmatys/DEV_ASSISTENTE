using System.Diagnostics;

namespace ASSISTENTE.Infrastructure.CodeParser.Contracts.Models;

[DebuggerDisplay("{Value}")]
public sealed class ModifierModel
{
    private ModifierModel(string value)
    {
        Value = value;
    }

    private string Value { get; }
    
    public static ModifierModel Create(string value)
    {
        return new ModifierModel(value);
    }
    
    public override string ToString()
    {
        return Value;
    }
}