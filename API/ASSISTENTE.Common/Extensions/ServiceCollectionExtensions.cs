using Microsoft.Extensions.DependencyInjection;

namespace ASSISTENTE.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static TResult GetSettings<TSettings, TResult>(
        this IServiceCollection services, 
        Func<TSettings, TResult> selector) 
        where TSettings : notnull
    {
        var serviceProvider = services.BuildServiceProvider();
        var settings = serviceProvider.GetRequiredService<TSettings>();
        return selector(settings);
    }
}