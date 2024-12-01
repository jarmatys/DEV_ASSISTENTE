using System.Text;
using System.Text.Json;
using ASSISTENTE.Application.Abstractions.Interfaces;
using ASSISTENTE.Application.Handlers.Knowledge.Commands;
using ASSISTENTE.Infrastructure.Firecrawl.Contracts;
using ASSISTENTE.Infrastructure.LLM.Contracts;
using ASSISTENTE.Playground.Models;
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
        var result = await Task_02();

        result
            .Log("Task completed!", logger)
            .LogError(logger);
    }

    private async Task<Result> Task_01()
    {
        const string url = "https://xyz.ag3nts.org";

        return await firecrawlService.ScrapeAsync(url)
            .Bind(markdownContent =>
                Prompt.Create(
                    $"Answer the question, extract only date without any extra infomation: {markdownContent}"))
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

    private async Task<Result> Task_02()
    {
        const string memory = "- The capital of Poland is Kraków." +
                              "- A well-known number from the book The Hitchhiker's Guide to the Galaxy is 69." +
                              "- The current year is 1999.";

        const string instruction = "Answer the question, extract only date without any extra infomation.";

        return await VerifyRequest()
            .Bind(verifyResponse =>
            {
                return Prompt
                    .Create($"{instruction} <memory>{memory}</memory> {verifyResponse.Text}")
                    .Bind(async prompt => await llmClient.GenerateAnswer(prompt))
                    .Bind(async answer => await VerifyRequest(verifyResponse.MessageId, answer.Text));
            })
            .Tap(result => Console.WriteLine(result.Text));
    }

    private async Task<Result<HumanCaptchaModel>> VerifyRequest(
        int? messageId = null,
        string? text = null)
    {
        const string url = "https://xyz.ag3nts.org/verify";

        var requestBody = messageId == null
            ? new HumanCaptchaModel
            {
                MessageId = 0,
                Text = "READY"
            }
            : new HumanCaptchaModel
            {
                MessageId = messageId.Value,
                Text = text ?? ""
            };

        var response = await httpClient.PostAsync(
            url,
            new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8, "application/json"
            )
        );

        if (!response.IsSuccessStatusCode)
            return Result.Failure<HumanCaptchaModel>("");

        var responseContent = await response.Content.ReadAsStringAsync();

        var parsedResponse = JsonSerializer.Deserialize<HumanCaptchaModel>(responseContent);

        return parsedResponse is null
            ? Result.Failure<HumanCaptchaModel>("")
            : Result.Success(parsedResponse);
    }
}