using System.Reflection;
using ASSISTENTE.Application;
using ASSISTENTE.Domain.Common.Interfaces;

namespace ASSISTENTE.Unit.Architecture.Common;

public abstract class ArchitectureTestBase
{
    protected Assembly DomainAssembly { get; } = typeof(IEntity).Assembly;
    protected Assembly ApplicationAssembly { get; } = typeof(ApplicationAssemblyMarker).Assembly;

    protected static string AssemblyName(Assembly assembly) => assembly.GetName().Name!;
}