using ASSISTENTE.UI.Common.Modules;

namespace ASSISTENTE.UI.Auth;

public class Module : IModule
{
    public static string Name => "AUTH Module";

    public List<NavItem> NavItems =>
    [
        new NavItem { Name = "Login", Url = "/auth/login" }
    ];
}