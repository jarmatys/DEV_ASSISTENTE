using System.Reflection;
using ASSISTENTE.Domain.Common;
using ASSISTENTE.Unit.Architecture.Common;
using FluentAssertions;
using NetArchTest.Rules;

namespace ASSISTENTE.Unit.Architecture;

public sealed class DomainEntitiesTests : ArchitectureTestBase
{
    [Fact]
    public void Entities_ShouldOnlyHave_PrivateConstructors()
    {
        var entityTypes = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity<>))
            .GetTypes();

        var failingTypes = new List<Type>();
        
        foreach (var entityType in entityTypes)
        {
            var constructors = entityType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

            if (constructors.Length != 0)
            {
                failingTypes.Add(entityType);
            }
        }

        failingTypes.Should().BeEmpty();
    }
}