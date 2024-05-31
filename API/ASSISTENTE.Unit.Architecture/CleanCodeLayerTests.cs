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
}