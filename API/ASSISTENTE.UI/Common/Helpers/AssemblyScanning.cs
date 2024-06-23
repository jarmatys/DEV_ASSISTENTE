using System.Reflection;
using ASSISTENTE.UI.Common.Modules;

namespace ASSISTENTE.UI.Common.Helpers;

public class AssemblyScanning
{
    public static List<Assembly> GetAssemblies()
    {
        var allAssemblies = new List<Assembly>();

        var assembly = Assembly.GetExecutingAssembly();
        
        allAssemblies.AddRange(assembly.GetReferencedAssemblies().Select(Assembly.Load));

        var returnAssemblies = allAssemblies
            .Where(w => w.GetTypes().Any(a => a.GetInterfaces().Contains(typeof(IModule))));

        return returnAssemblies.ToList();
    }
}