using System.Text;
using System.Text.Json;
using ASSISTENTE.Infrastructure.Audio.Contracts;
using ASSISTENTE.Infrastructure.LLM.Contracts;
using ASSISTENTE.Infrastructure.Vision.Contracts;
using ASSISTENTE.Playground.Models;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Playground.Tasks;

public class WeekTwo(
    HttpClient httpClient,
    ILlmClient llmClient,
    IAudioClient audioClient,
    IVisionClient visionClient)
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
    
    public async Task<Result> Task_02()
    {
        const string audioFilePath = "Data/Images";
        
        var filePaths = Directory.GetFiles(audioFilePath);
        
        var imageDescriptions = new StringBuilder();

        const string imagePrompt = "Rozpoznaj i znajduje się na otrzymanym fragmencie mapy, istotne " +
                                   "są nazwy ulic i obiektów na mapie. Odpowiedz w języku Polskim.";
        
        var tasks = new List<Task>();
        foreach (var filePath in filePaths)
        {
            await using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            using var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream);
            var byteImage = memoryStream.ToArray();

            var base64Image = Convert.ToBase64String(byteImage);
            var extension = Path.GetExtension(filePath);
            
            var task = Image.Create(imagePrompt, base64Image, extension)
                .Bind(visionClient.Recognize)
                .Tap(recognition =>
                {
                    imageDescriptions.Append($"Opis fragmentu mapy: \n{recognition.Text} \n\n");
                });
            
            tasks.Add(task);
        }
        await Task.WhenAll(tasks);

        var context = imageDescriptions.ToString();

        const string instuctions =
            "1. Na podstawie otrzymanych opisów ulic i miejsc określ prawdopodobne miasto z którego pochodzą." +
            "2. Pamiętaj, że jeden z opisów mapy może być błędny i może pochodzić z innego miasta.";

        const string step1 = "Przygotuj listę miast których mogą dotyczyć wybrane fragmenty i " +
                             "uzasadnij dlaczego je wybrałeś. Nie sugeruj wyboru. Przedstaw fakty. " +
                             "Nie przygotowywuj podsumowania. Same fakty.";

        const string step2 = "Zwróć tylko i wyłącznie jedno miasto, które jest najbardziej " +
                             "prawdopodobne, zwróć uwage na fakty jasno identyfikujące dane miasto, " +
                             "np. numery ulicy, obiekt przy tej ulicy. Nie kieruj się domysłami. Interesują Cię " +
                             "tylko kluczowe fakty.";
        
        var answer = await Prompt.Create($"<INSTRUKCJE>{instuctions}</INSTRUKCJE>\n\n <OPIS>{context}</OPIS>")
            .Bind(async prompt => await llmClient.GenerateAnswer(prompt))
            .Bind(answer => Prompt.Create($"Na podstawie otrzymanych informacji {step1} \n\n <INFORMACJE>{answer.Text}</INFORMACJE>."))
            .Bind(async prompt => await llmClient.GenerateAnswer(prompt))
            .Bind(answer => Prompt.Create($"Na podstawie otrzymanych informacji {step2} \n\n <INFORMACJE>{answer.Text}</INFORMACJE>."))
            .Bind(async prompt => await llmClient.GenerateAnswer(prompt));
        
        Console.WriteLine($"Miasto: {answer.GetValueOrDefault(x => x.Text)}");
        
        return Result.Success();
    }
}