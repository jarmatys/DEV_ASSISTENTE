using System.Reflection;
using NetArchTest.Rules;
using Shouldly;

namespace ASSISTENTE.Unit.Architecture.Common;

public static class TestsExtensions
{
    public static void ShouldBeSuccessful(this TestResult result)
    {
        result.IsSuccessful.ShouldBeTrue();
    }
    
    public static TestResult ShoudlNotHaveDependencyOn(this Assembly assembly, string dependency)
    {
        return Types.InAssembly(assembly)
            .Should()
            .NotHaveDependencyOn(dependency)
            .GetResult();
    }
}