using ASSISTENTE.Unit.Architecture.Common;

namespace ASSISTENTE.Unit.Architecture;

public sealed class CleanCodeLayerTests : ArchitectureTestBase
{
    [Fact]
    public void DomainLayer_ShouldNotHaveDependencyOn_ApplicationLayer()
    {
        DomainAssembly
            .ShoudlNotHaveDependencyOn(AssemblyName(ApplicationAssembly))
            .ShouldBeSuccessful();
    }

    [Fact]
    public void DomainLayer_ShouldNotHaveDependencyOn_PersistenceLayer()
    {
        DomainAssembly
            .ShoudlNotHaveDependencyOn(AssemblyName(PersistenceAssembly))
            .ShouldBeSuccessful();
    }

    [Fact]
    public void DomainLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
    {
        DomainAssembly
            .ShoudlNotHaveDependencyOn(AssemblyName(InfrastructureAssembly))
            .ShouldBeSuccessful();
    }
    
    [Fact]
    public void PersistenceAssembly_ShouldNotHaveDependencyOn_InfrastructureLayer()
    {
        PersistenceAssembly
            .ShoudlNotHaveDependencyOn(AssemblyName(InfrastructureAssembly))
            .ShouldBeSuccessful();
    }
    
    [Fact]
    public void ApplicationAssembly_ShouldNotHaveDependencyOn_InfrastructureLayer()
    {
        ApplicationAssembly
            .ShoudlNotHaveDependencyOn(AssemblyName(InfrastructureAssembly))
            .ShouldBeSuccessful();
    }
    
    [Fact]
    public void ApplicationAssembly_ShouldNotHaveDependencyOn_PersistenceAssembly()
    {
        ApplicationAssembly
            .ShoudlNotHaveDependencyOn(AssemblyName(PersistenceAssembly))
            .ShouldBeSuccessful();
    }
    
    [Fact]
    public void EventsAssembly_ShouldNotHaveDependencyOn_PersistenceAssembly()
    {
        EventsAssembly
            .ShoudlNotHaveDependencyOn(AssemblyName(PersistenceAssembly))
            .ShouldBeSuccessful();
    }
    
    [Fact]
    public void EventsAssembly_ShouldNotHaveDependencyOn_ApplicationAssembly()
    {
        EventsAssembly
            .ShoudlNotHaveDependencyOn(AssemblyName(ApplicationAssembly))
            .ShouldBeSuccessful();
    }
    
    [Fact]
    public void EventsAssembly_ShouldNotHaveDependencyOn_InfrastructureAssembly()
    {
        EventsAssembly
            .ShoudlNotHaveDependencyOn(AssemblyName(InfrastructureAssembly))
            .ShouldBeSuccessful();
    }
}