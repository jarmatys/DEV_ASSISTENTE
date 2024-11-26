using ASSISTENTE.Application.Abstractions.Interfaces;
using ASSISTENTE.Application.Handlers.Knowledge.Commands;
using ASSISTENTE.Infrastructure.Firecrawl.Contracts;
using ASSISTENTE.Infrastructure.LLM.Contracts;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using SOFTURE.Common.Logging.Extensions;

namespace ASSISTENTE.Playground;

public sealed class Playground(
    IKnowledgeService knowledgeService,
    IMaintenanceService maintenanceService,
    IFirecrawlService firecrawlService,
    ILlmClient llmClient,
    HttpClient httpClient,
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
    
    public async Task RunAsync()
    {
        const string url = "https://xyz.ag3nts.org";

        var result = await firecrawlService.ScrapeAsync(url)
            .Bind(markdownContent => Prompt.Create($"Answer the question, extract only date without any extra infomation: {markdownContent}"))
            .Bind(async prompt => await llmClient.GenerateAnswer(prompt))
            .Bind(async answer =>
            {
                const string userName = "tester";
                const string password = "574e112a";

                var formData = new Dictionary<string, string>
                {
                    { "username", userName },
                    { "password", password },
                    { "answer", answer.Text }
                };

                var response = await httpClient.PostAsync(url, new FormUrlEncodedContent(formData));

                var responseContent = await response.Content.ReadAsStringAsync();
                
                return response.IsSuccessStatusCode
                    ? Result.Success(responseContent)
                    : Result.Failure("");
            });
    }
}