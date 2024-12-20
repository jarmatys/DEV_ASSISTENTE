using System.Text;
using System.Text.Json;
using ASSISTENTE.Infrastructure.Audio.Contracts;
using ASSISTENTE.Infrastructure.LLM.Contracts;
using ASSISTENTE.Playground.Models;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Playground.Tasks;

public class WeekTwo(
    HttpClient httpClient,
    ILlmClient llmClient,
    IAudioClient audioClient)
{
    public async Task<Result> Task_01()
    {
        const string url = "https://centrala.ag3nts.org/report";
        const string taskName = "mp3";
        const string apiKey = "<API-KEY>";
        const string audioFilePath = "Data/Audio";
        
        var filePaths = Directory.GetFiles(audioFilePath);
        
        var transcriptions = new StringBuilder();

        var tasks = new List<Task>();
        foreach (var filePath in filePaths)
        {
            var fileName = Path.GetFileName(filePath);
            
            await using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        
            var task = AudioFile.Create(fileName, fileStream)
                .Bind(audioClient.GenerateTranscription)
                .Tap(transcription =>
                {
                    transcriptions.Append($"Zeznanie z pliku: {fileName}\n");
                    transcriptions.Append(transcription.Text);
                    transcriptions.Append("\n\n");
                });
            
            tasks.Add(task);
        }
        await Task.WhenAll(tasks);

        var context = transcriptions.ToString();
        
        const string instuctions =
            "1. Znajdź odpowiedź na pytanie, na jakiej ulicy znajduje się uczelnia, na której wykłada Andrzej Maj" +
            "2. Pamiętaj, że zeznania świadków mogą być sprzeczne, niektórzy z nich mogą się mylić, a inni odpowiadać w dość dziwaczny sposób." +
            "3. Nazwa ulicy nie pada w treści transkrypcji. Musisz użyć wiedzy wewnętrznej modelu, aby uzyskać odpowiedź.";

        var answer = await Prompt.Create($"<INSTRUKCJE>{instuctions}</INSTRUKCJE>\n\n <ZEZNANIA>{context}</ZEZNANIA>")
            .Bind(async prompt => await llmClient.GenerateAnswer(prompt))
            .Bind(answer => Prompt.Create($"Na podstawie otrzymanych informacji: \n <INFORMACJE>{answer.Text}</INFORMACJE>. \n\n Zwróć tylko i wyłącznie nazwę ulicy przy której znajduje się zidentifikowane miejsce."))
            .Bind(async prompt => await llmClient.GenerateAnswer(prompt));

        var request = new TaskRequestModel
        {
            Task = taskName,
            ApiKey = apiKey,
            Answer = answer.GetValueOrDefault(x => x.Text)
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
            : Result.Failure(responseContent);
    }
}