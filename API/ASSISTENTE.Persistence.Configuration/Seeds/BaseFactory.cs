using System.Reflection;

namespace ASSISTENTE.Persistence.Configuration.Seeds;

public abstract class BaseFactory
{
    protected static T CreateInstance<T>(params object[] constructorParameters)
    {
        var privateConstructorParameters = constructorParameters.ToList();

        var constructorParameterTypes = privateConstructorParameters
            .Select(pcp => pcp.GetType())
            .ToArray();

        var type = typeof(T);

        var constructor = type.GetConstructor(
            BindingFlags.NonPublic | BindingFlags.Instance,
            null,
            constructorParameterTypes,
            null
        );

        if (constructor == null) throw new Exception($"Constructor not found for type {type.Name}");

        var entityInstance = (T)constructor.Invoke(privateConstructorParameters.ToArray());

        return entityInstance;
    }
}