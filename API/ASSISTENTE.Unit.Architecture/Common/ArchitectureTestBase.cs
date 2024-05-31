using System.Reflection;
using ASSISTENTE.Application;
using ASSISTENTE.Domain.Common.Interfaces;
using ASSISTENTE.EventHandlers;
using ASSISTENTE.Infrastructure;
using ASSISTENTE.Persistence;

namespace ASSISTENTE.Unit.Architecture.Common;

public abstract class ArchitectureTestBase
{
    protected Assembly DomainAssembly { get; } = typeof(IEntity).Assembly;
    protected Assembly ApplicationAssembly { get; } = typeof(ApplicationAssemblyMarker).Assembly;
    protected Assembly PersistenceAssembly { get; } = typeof(PersistenceAssemblyMarker).Assembly;
    protected Assembly InfrastructureAssembly { get; } = typeof(InfrastructureAssemblyMarker).Assembly;
    protected Assembly EventsAssembly { get; } = typeof(EventsAssemblyMarker).Assembly;

    protected static string AssemblyName(Assembly assembly) => assembly.GetName().Name!;
}