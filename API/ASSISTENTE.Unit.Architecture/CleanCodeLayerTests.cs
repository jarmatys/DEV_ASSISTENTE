using ASSISTENTE.Unit.Architecture.Common;

namespace ASSISTENTE.Unit.Architecture;

public class CleanCodeLayerTests : ArchitectureTestBase
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
}