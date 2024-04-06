using ASSISTENTE.Application.Knowledge.Commands.Learn;
using ASSISTENTE.Application.Knowledge.Queries.Answer;
using ASSISTENTE.Application.Maintenance.Commands.Reset;
using ASSISTENTE.Common.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ASSISTENTE.Playground;

public sealed class Playground(ISender mediator, ILogger<Playground> logger)
{
    public async Task AnswerAsync(string question)
    {
        var result = await mediator.Send(AnswerQuery.Create(question));

        result
            .Log(response => response.Text, logger)
            .LogError(logger);
    }

    public async Task LearnAsync()
    {
        var result = await mediator.Send(LearnCommand.Create());

        result
            .Log("Learning completed!", logger)
            .LogError(logger);
    }

    public async Task ResetAsync()
    {
       var result = await mediator.Send(ResetCommand.Create());

        result
            .LogError(logger);
    }
}