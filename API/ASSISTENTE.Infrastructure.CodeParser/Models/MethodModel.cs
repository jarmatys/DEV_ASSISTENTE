using System.Diagnostics;

namespace ASSISTENTE.Infrastructure.CodeParser.Models;

[DebuggerDisplay("{ReturnName} {Name}")]
public sealed class MethodModel
{
    private MethodModel(
        string name, 
        string returnName, 
        string body, 
        IEnumerable<ModifierModel> modifiers, 
        IEnumerable<ParameterModel> parameter)
    {
        Name = name;
        ReturnName = returnName;
        Body = body;
        Modifiers = modifiers;
        Parameter = parameter;
    }

    private string Name { get; }
    private string ReturnName { get; }
    private string Body { get; }
    private IEnumerable<ModifierModel> Modifiers { get; }
    private IEnumerable<ParameterModel> Parameter { get; }
    
    public static MethodModel Create(
        string name, 
        string returnName, 
        string body, 
        IEnumerable<ModifierModel> modifiers, 
        IEnumerable<ParameterModel> parameter)
    {
        return new MethodModel(name, returnName, body, modifiers, parameter);
    }
    
    public override string ToString()
    {
        var modifiers = string.Join(" ", Modifiers);
        var parameters = string.Join(", ", Parameter);
        
        return $"{modifiers} {ReturnName} {Name}({parameters})\n{Body}";
    }
}