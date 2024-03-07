namespace ASSISTENTE.Infrastructure.CodeParser.Models;

internal sealed class ModifierModel
{
    private ModifierModel(string value)
    {
        Value = value;
    }
    
    public string Value { get; }
    
    public static ModifierModel Create(string value)
    {
        return new ModifierModel(value);
    }
}