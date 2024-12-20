using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ASSISTENTE.Infrastructure.Firecrawl.Contracts;
using ASSISTENTE.Infrastructure.Langfuse.Contracts;
using ASSISTENTE.Infrastructure.LLM.Contracts;
using ASSISTENTE.Playground.Models;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Playground.Tasks;

public class WeekTwo(
    HttpClient httpClient,
    ILlmClient llmClient)
{
    public async Task<Result> Task_01()
    {
        const string url = "https://centrala.ag3nts.org/report";
        const string taskName = "JSON";
        const string apiKey = "<API-KEY>";
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
            ApiKey = apiKey,
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
}