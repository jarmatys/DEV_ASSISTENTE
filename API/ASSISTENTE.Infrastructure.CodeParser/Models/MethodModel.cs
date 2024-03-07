namespace ASSISTENTE.Infrastructure.CodeParser.Models;

internal sealed class MethodModel
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
    
    public string Name { get; }
    public string ReturnName { get; }
    public string Body { get; }
    public IEnumerable<ModifierModel> Modifiers { get; }
    public IEnumerable<ParameterModel> Parameter { get; }
    
    public static MethodModel Create(
        string name, 
        string returnName, 
        string body, 
        IEnumerable<ModifierModel> modifiers, 
        IEnumerable<ParameterModel> parameter)
    {
        return new MethodModel(name, returnName, body, modifiers, parameter);
    }
}