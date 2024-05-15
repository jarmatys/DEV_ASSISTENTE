using ASSISTENTE.Application.Abstractions.Interfaces;
using ASSISTENTE.Application.Handlers.Knowledge.Commands;
using ASSISTENTE.Application.Handlers.Maintenance.Commands;
using ASSISTENTE.Common.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ASSISTENTE.Playground;

public sealed class Playground(
    IKnowledgeService knowledgeService,
    IMaintenanceService maintenanceService,
    ISender mediator,
    ILogger<Playground> logger)
{
    public async Task AnswerAsync(string question)
    {
        var answerResult = await knowledgeService.AnswerAsync(question);

        answerResult
            .Log(text => text, logger)
            .LogError(logger);
    }

    public async Task LearnAsync()
    {
        var init = await maintenanceService.InitAsync();

        init
            .Log("Enviroment initialized!", logger)
            .LogError(logger);

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