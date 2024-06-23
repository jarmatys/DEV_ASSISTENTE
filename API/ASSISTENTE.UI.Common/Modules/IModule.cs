namespace ASSISTENTE.UI.Common.Modules;

public interface IModule
{
    static string Name { get; } = null!;

    List<NavItem> NavItems { get; }
}