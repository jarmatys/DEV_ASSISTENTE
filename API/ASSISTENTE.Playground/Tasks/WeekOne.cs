using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ASSISTENTE.Infrastructure.Firecrawl.Contracts;
using ASSISTENTE.Infrastructure.LLM.Contracts;
using ASSISTENTE.Playground.Models;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Playground.Tasks;

public class WeekOne(
    HttpClient httpClient,
    IFirecrawlService firecrawlService,
    ILlmClient llmClient)
{
    private const string ApiKey = "<API-KEY>";
    
    public async Task<Result> Task_01()
    {
        const string url = "https://xyz.ag3nts.org";

        return await firecrawlService.ScrapeAsync(url)
            .Bind(markdownContent => Prompt.Create($"Answer the question, extract " +
                                                   $"only date without any extra infomation: {markdownContent}"))
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

    public async Task<Result> Task_02()
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

    public async Task<Result> Task_03()
    {
        // LanguageFuse is a library to monitor the language model performance.
        // TODO: library https://langfuse.com/

        const string url = "https://centrala.ag3nts.org/report";
        const string taskName = "JSON";
        const string filePath = "Data/data.json";

        var fileContent = await File.ReadAllTextAsync(filePath);
        var parsedFile = JsonSerializer.Deserialize<DataModel>(fileContent);

        if (parsedFile is null)
            return Result.Failure("Failed to parse data file.");

        foreach (var item in parsedFile.Data)
        {
            if (item.AdditionalInformation is not null)
            {
                var answer = await Prompt.Create($"{item.AdditionalInformation.Question}")
                    .Bind(async prompt => await llmClient.GenerateAnswer(prompt));

                item.AdditionalInformation.Answer = answer.Value.Text;
            }

            var calculation = item.Question.Split(" + ").Select(int.Parse).Sum();
            if (calculation != item.Answer)
            {
                item.Answer = calculation;
            }
        }

        var updatedFileContent = JsonSerializer.Serialize(parsedFile, new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });

        await File.WriteAllTextAsync(filePath, updatedFileContent);

        var request = new TaskRequestModel
        {
            Task = taskName,
            ApiKey = ApiKey,
            Answer = parsedFile
        };

        var response = await httpClient.PostAsync(
            url,
            new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8, "application/json"
            )
        );

        var responseContent = await response.Content.ReadAsStringAsync();

        return response.IsSuccessStatusCode
            ? Result.Success(responseContent)
            : Result.Failure("");
    }

    public async Task<Result> Task_05()
    {
        const string fileUrl = $"https://centrala.ag3nts.org/data/{ApiKey}/cenzura.txt";

        var fileResponse = await httpClient.GetAsync(fileUrl);
        var fileContent = await fileResponse.Content.ReadAsStringAsync();

        const string taskName = "CENZURA";

        var result = await Prompt.Create($"{fileContent}")
            .Bind(async prompt => await llmClient.GenerateAnswer(prompt));

        var request = new TaskRequestModel
        {
            Task = taskName,
            ApiKey = ApiKey,
            Answer = result.Value.Text
        };

        const string url = "https://centrala.ag3nts.org/report";

        var response = await httpClient.PostAsync(
            url,
            new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8, "application/json"
            )
        );

        var responseContent = await response.Content.ReadAsStringAsync();

        return response.IsSuccessStatusCode
            ? Result.Success()
            : Result.Failure(responseContent);
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