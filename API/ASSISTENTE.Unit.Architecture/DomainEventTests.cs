using ASSISTENTE.Domain.Common.Interfaces;
using ASSISTENTE.Unit.Architecture.Common;
using NetArchTest.Rules;

namespace ASSISTENTE.Unit.Architecture;

public sealed class DomainEventTests : ArchitectureTestBase
{
    [Fact]
    public void DomainEvents_Should_BeSealed()
    {
        Types.InAssembly(DomainAssembly)
            .That()
            .ImplementInterface(typeof(IDomainEvents))
            .Should()
            .BeSealed()
            .GetResult()
            .ShouldBeSuccessful();
    }
}