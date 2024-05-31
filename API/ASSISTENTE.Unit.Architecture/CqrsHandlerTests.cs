using ASSISTENTE.Unit.Architecture.Common;
using MediatR;
using NetArchTest.Rules;

namespace ASSISTENTE.Unit.Architecture;

public sealed class CqrsHandlerTests : ArchitectureTestBase
{
    [Fact]
    public void CQRS_Handler_ShouldHave_NameEndingWith_Handler()
    {
        Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(IRequestHandler<,>))
            .And()
            .AreNotAbstract()
            .Should()
            .HaveNameEndingWith("Handler")
            .GetResult()
            .ShouldBeSuccessful();
    }
}