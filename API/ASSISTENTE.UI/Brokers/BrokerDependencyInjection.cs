using ASSISTENTE.UI.Brokers.Implementations;

namespace ASSISTENTE.UI.Brokers;

public static class DependencyInjection
{
    public static IServiceCollection AddBrokers(this IServiceCollection services)
    {
        services.AddScoped<IAnswersBroker, AnswersBroker>();
        services.AddScoped<IQuestionsBroker, QuestionsBroker>();

        return services;
    }
}