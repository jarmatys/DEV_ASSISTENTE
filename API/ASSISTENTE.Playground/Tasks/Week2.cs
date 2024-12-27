using System.Text;
using ASSISTENTE.Infrastructure.Audio.Contracts;
using ASSISTENTE.Infrastructure.Firecrawl.Contracts;
using ASSISTENTE.Infrastructure.Image.Contracts;
using ASSISTENTE.Infrastructure.LLM.Contracts;
using ASSISTENTE.Infrastructure.MarkDownParser.Contracts;
using ASSISTENTE.Infrastructure.Vision.Contracts;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Playground.Tasks;

public class Week2(
    HttpClient httpClient,
    ILlmClient llmClient,
    IAudioClient audioClient,
    IVisionClient visionClient,
    IImageClient imageClient,
    IFirecrawlService firecrawlService,
    IMarkDownParser markDownParser) : TaskBase(httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<Result<string>> Task_01()
    {
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

        return await ReportResult("mp3", answer.GetValueOrDefault(x => x.Text));
    }

    public async Task<Result<string>> Task_02()
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

            var task = VisionImage.Create(imagePrompt, base64Image, extension)
                .Bind(visionClient.Recognize)
                .Tap(recognition => { imageDescriptions.Append($"Opis fragmentu mapy: \n{recognition.Text} \n\n"); });

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
        
        return Result.Success($"Miasto: {answer.GetValueOrDefault(x => x.Text)}");
    }

    public async Task<Result<string>> Task_03()
    {
        const string robotDescriptionUrl = $"https://centrala.ag3nts.org/data/{ApiKey}/robotid.json";

        var fileResponse = await _httpClient.GetAsync(robotDescriptionUrl);
        var robotDescription = await fileResponse.Content.ReadAsStringAsync();

        var image = await ImagePrompt.Create(robotDescription)
            .Bind(async prompt => await imageClient.GenerateImage(prompt));

        return await ReportResult("robotid", image.GetValueOrDefault(x => x.ImageUrl));
    }

    public async Task<Result<string>> Task_04()
    {
        const string filesPath = "Data/Files";

        const string masterPrompt = "Twoim zadaniem jest na podstawie treści podanej w <OPIS> zwrócić odpowiednie " +
                                    "słowo kluczowe: HARDWARE, PEOPLE, MISSING.\n\n" +
                                    "Przykład: 'Czujniki dźwięku wykryły ultradźwiękowy sygnał, pochodzenie: nadajnik ukryty w zielonych krzakach'\n" +
                                    "Zwracasz: HARDWARE\n\n" +
                                    "Przykład: Osobnik przedstawił się jako Aleksander Ragowski." +
                                    "Zwracasz: PEOPLE\n\n";

        const string rules = "1. Jeżeli w danym pliku znajdują się informacje o sprzęcie to ZWRACASZ SŁOWO: HARDWARE" +
                             "2. Jeżeli w danym pliku znajdują się informacje o ludziach to ZWRACASZ SŁOWO: PEOPLE" +
                             "3. Istnieje szansa, że znaleziona informacja nie dotyczy ani ludzi, ani maszyn! ZWRACASZ SŁOWO: MISSING";

        var filePaths = Directory.GetFiles(filesPath);

        var peopleList = new List<string>();
        var hardwareList = new List<string>();

        foreach (var filePath in filePaths)
        {
            var fileName = Path.GetFileName(filePath);
            var fileExtension = Path.GetExtension(filePath);

            if (fileExtension == ".txt")
            {
                var fileText = await File.ReadAllTextAsync(filePath);

                var promptText = $"<ZADANIE>{masterPrompt}</ZADANIE\n\n" +
                                 $"<ZASADY>{rules}</ZASADY>\n\n" +
                                 $"<OPIS>{fileText}</OPIS>";

                var answer = await Prompt.Create(promptText)
                    .Bind(async prompt => await llmClient.GenerateAnswer(prompt))
                    .GetValueOrDefault(x => x.Text);

                if (answer == "PEOPLE")
                    peopleList.Add(fileName);

                if (answer == "HARDWARE")
                    hardwareList.Add(fileName);
            }

            if (fileExtension == ".mp3")
            {
                await using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                var answer = await AudioFile.Create(fileName, fileStream)
                    .Bind(async prompt => await audioClient.GenerateTranscription(prompt))
                    .Map(transcription => $"<ZADANIE>{masterPrompt}</ZADANIE\n\n" +
                                          $"<ZASADY>{rules}</ZASADY>\n\n" +
                                          $"<OPIS>{transcription.Text}</OPIS>")
                    .Bind(Prompt.Create)
                    .Bind(async prompt => await llmClient.GenerateAnswer(prompt))
                    .GetValueOrDefault(x => x.Text);

                if (answer == "PEOPLE")
                    peopleList.Add(fileName);

                if (answer == "HARDWARE")
                    hardwareList.Add(fileName);
            }

            if (fileExtension == ".png")
            {
                await using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                using var memoryStream = new MemoryStream();
                await fileStream.CopyToAsync(memoryStream);
                var byteImage = memoryStream.ToArray();

                var base64Image = Convert.ToBase64String(byteImage);
                var extension = Path.GetExtension(filePath);

                var answer = await VisionImage.Create("Zwróć treść z obrazka", base64Image, extension)
                    .Bind(async prompt => await visionClient.Recognize(prompt))
                    .Map(recognition => $"<ZADANIE>{masterPrompt}</ZADANIE\n\n" +
                                        $"<ZASADY>{rules}</ZASADY>\n\n" +
                                        $"<OPIS>{recognition.Text}</OPIS>")
                    .Bind(Prompt.Create)
                    .Bind(async prompt => await llmClient.GenerateAnswer(prompt))
                    .GetValueOrDefault(x => x.Text);

                if (answer == "PEOPLE")
                    peopleList.Add(fileName);

                if (answer == "HARDWARE")
                    hardwareList.Add(fileName);
            }
        }

        var result = new
        {
            people = peopleList,
            hardware = hardwareList
        };
        
        return await ReportResult("kategorie", result);
    }

    public async Task<Result<string>> Task_05()
    {
        const string articleUrl = "https://centrala.ag3nts.org/dane";
        const string questionsUrl = $"https://centrala.ag3nts.org/data/{ApiKey}/arxiv.txt";

        const string markdownContentPath = "Data/firecrawl-example.md";

        const string masterPrompt = "Na podstawie otrzymanego kontekstu <CONTEXT> odpowiedz " +
                                    "na pytania podane w tagu <PYTANIA>. Odpowiedz krótko na temat i " +
                                    "zwięźle. Max jedno zdanie.";

        // var markdownContent = await firecrawlService.ScrapeAsync($"{articleUrl}/arxiv-draft.html");

        var context = new StringBuilder();

        var fileContent = await File.ReadAllTextAsync(markdownContentPath);
        
        context.Append($"Informacje pobrane ze strony: {fileContent} \n\n");
        
        await FilePath.Create(markdownContentPath)
            .Bind(markDownParser.GetMediaUrls)
            .Bind(async mediaUrls =>
            {
                foreach (var mediaUrl in mediaUrls)
                {
                    var mediaFullUrl = $"{articleUrl}/{mediaUrl.Url}";
                    
                    if (mediaUrl.Extension == "mp3")
                    {
                        var file = await _httpClient.GetByteArrayAsync(mediaFullUrl);
                        
                        await using var fileStream = new MemoryStream(file);
                        
                        var answer = await AudioFile.Create(mediaUrl.Url, fileStream)
                            .Bind(async prompt => await audioClient.GenerateTranscription(prompt))
                            .GetValueOrDefault(x => x.Text);
                        
                        context.Append($"Informacje pobrane z pliku: '{mediaUrl.Url}': \n{answer}\n\n");
                    }
        
                    if (mediaUrl.Extension == "png")
                    {
                        var file = await _httpClient.GetByteArrayAsync(mediaFullUrl);
                    
                        var base64Image = Convert.ToBase64String(file);
                        
                        var answer = await VisionImage.Create("Opisz co widzisz na obrazku w języku polskim", base64Image, "png")
                            .Bind(async prompt => await visionClient.Recognize(prompt))
                            .GetValueOrDefault(x => x.Text);
                        
                        context.Append($"Informacje pobrane z pliku: '{mediaUrl.Url}': \n{answer}\n\n");
                    }
                }
        
                return Result.Success();
            });

        var fullContext = context.ToString();
           
        var questions = await _httpClient.GetStringAsync(questionsUrl);

        var questionsList = questions
            .Split("\n").Where(q => q != string.Empty)
            .ToList();

        var finalAnswers = new Dictionary<string, string>();

        foreach (var question in questionsList)
        {
            var questionId = question.Split("=")[0];
            var questionText = question.Split("=")[1];

            await Prompt.Create($"<INSTRUKCJE>{masterPrompt}</INSTRUKCJE>" +
                                $"<CONTEXT>{fullContext}</CONTEXT>" +
                                $"<PYTANIE>{questionText}</PYTANIE>")
                .Bind(async prompt => await llmClient.GenerateAnswer(prompt))
                .Tap(answer => finalAnswers.Add(questionId, answer.Text));
        }

        return await ReportResult("arxiv", finalAnswers);
    }
}