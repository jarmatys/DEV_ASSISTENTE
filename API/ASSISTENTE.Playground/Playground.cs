using ASSISTENTE.Application.Knowledge.Commands.Learn;
using ASSISTENTE.Application.Knowledge.Queries.Answer;
using ASSISTENTE.Application.Maintenance.Commands.Reset;
using CSharpFunctionalExtensions;
using MediatR;

namespace ASSISTENTE.Client;

public sealed class Playground(ISender mediator)
{
    public async Task AnswerAsync(string question)
    {
        var result = await mediator.Send(AnswerQuery.Create(question));

        result
            .Tap(response => Console.WriteLine($"\nAnswer: {response.Text}"))
            .TapError(errors => Console.WriteLine(errors));
    }

    public async Task LearnAsync()
    {
        var result = await mediator.Send(LearnCommand.Create());

        result
            .Tap(() => Console.WriteLine("\nLearning completed!"))
            .TapError(errors => Console.WriteLine(errors));
    }

    public async Task ResetAsync()
    {
       var result = await mediator.Send(ResetCommand.Create());

        result
            .TapError(errors => Console.WriteLine(errors));
    }
}