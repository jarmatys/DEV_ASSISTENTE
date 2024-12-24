using System.Text;
using System.Text.Json;
using ASSISTENTE.Playground.Models;
using ASSISTENTE.Playground.Models.CentralModels;
using ASSISTENTE.Playground.Models.DataApiModels;
using ASSISTENTE.Playground.Models.DatabaseModels;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Playground.Tasks;

public abstract class TaskBase(HttpClient httpClient)
{
    protected const string ApiKey = "<API_KEY>";

    protected async Task<Result<HumanCaptchaModel>> VerifyRequest(
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

    protected async Task<Result<string>> ReportResult(string taskName, object? taskResult)
    {
        const string url = "https://centrala.ag3nts.org/report";

        var request = new TaskRequestModel
        {
            Task = taskName,
            ApiKey = ApiKey,
            Answer = taskResult
        };

        var response = await httpClient.PostAsync(
            url,
            new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8, "application/json"
            )
        );

        var responseContent = await response.Content.ReadAsStringAsync();

        var deserializedContent = JsonSerializer.Deserialize<TaskResponse>(responseContent);

        return !response.IsSuccessStatusCode
            ? Result.Failure<string>(deserializedContent?.Message)
            : Result.Success(deserializedContent!.Message);
    }

    protected async Task<Result<string>> DatabaseQuery(string taskName, string query)
    {
        const string databaseUrl = "https://centrala.ag3nts.org/apidb";

        var request = new DatabaseRequestModel
        {
            Task = taskName,
            ApiKey = ApiKey,
            Query = query
        };

        var response = await httpClient.PostAsync(
            databaseUrl,
            new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8, "application/json"
            )
        );

        var responseContent = await response.Content.ReadAsStringAsync();

        var deserializedContent = JsonSerializer.Deserialize<DatabaseResponse>(responseContent);

        return !response.IsSuccessStatusCode
            ? Result.Failure<string>(deserializedContent?.Error)
            : Result.Success(deserializedContent!.Reply.ToString())!;
    }

    protected async Task<Result<string>> DataApiQuery(string source, string query)
    {
        const string peopleUrl = "https://centrala.ag3nts.org/people";
        const string placesUrl = "https://centrala.ag3nts.org/places";
        
        var request = new DataApiRequestModel
        {
            ApiKey = ApiKey,
            Query = query
        };

        HttpResponseMessage response;
        
        if (source == "PEOPLE")
        {
            response = await httpClient.PostAsync(
                peopleUrl,
                new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8, "application/json"
                )
            );
        }
        else if (source == "PLACES")
        {
            response = await httpClient.PostAsync(
                placesUrl,
                new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8, "application/json"
                )
            );
        }
        else
        {
            return Result.Failure<string>("Invalid source");
        }
        
        var responseContent = await response.Content.ReadAsStringAsync();
        
        var deserializedContent = JsonSerializer.Deserialize<DataApiResponse>(responseContent);

        return !response.IsSuccessStatusCode
            ? Result.Failure<string>(deserializedContent?.Message)
            : Result.Success(deserializedContent!.Message);
    }
}