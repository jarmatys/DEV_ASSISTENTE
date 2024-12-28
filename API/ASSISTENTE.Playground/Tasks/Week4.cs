using System.Diagnostics;
using System.Text;
using System.Text.Json;
using ASSISTENTE.Infrastructure.Audio.Contracts;
using ASSISTENTE.Infrastructure.Embeddings.Contracts;
using ASSISTENTE.Infrastructure.Firecrawl.Contracts;
using ASSISTENTE.Infrastructure.Image.Contracts;
using ASSISTENTE.Infrastructure.LLM.Contracts;
using ASSISTENTE.Infrastructure.MarkDownParser.Contracts;
using ASSISTENTE.Infrastructure.Neo4J.Contracts;
using ASSISTENTE.Infrastructure.Pdf4Me.Contracts;
using ASSISTENTE.Infrastructure.Qdrant.Contracts;
using ASSISTENTE.Infrastructure.Vision.Contracts;
using ASSISTENTE.Playground.Models;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Playground.Tasks;

public class Week4(
    HttpClient httpClient,
    ILlmClient llmClient,
    IAudioClient audioClient,
    IVisionClient visionClient,
    IImageClient imageClient,
    IFirecrawlService firecrawlService,
    IMarkDownParser markDownParser,
    IEmbeddingClient embeddingClient,
    INeo4JService neo4JService,
    IQdrantService qdrantService,
    IPdf4MeService pdf4MeService) : TaskBase(httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<Result<string>> Task_01()
    {
        // TODO: Prompt wybierający metody do rozwiązania zadania
        // https://github.com/i-am-alice/3rd-devs/tree/main/todo/prompts

        var photos = await ReportResult("photos", taskResult: "START");
        
        // STEP 1. Rozpoznaj linki ze zdjęciami i zwróć w formie listy po przecinku
        var imageUrl = "https://centrala.ag3nts.org/dane/barbara/IMG_1443.PNG";
        var imageName = imageUrl.Split('/').Last();

        // STEP 2. Uruchom OCR na zdjęciach i zweryfikuj co jest uszkodzone
        const string visionPrompt = """
                                    Otrzymałeś uszkodze zdjęcie, podejmij decyzję jakiej akcji użyć aby naprawić zdjęcie.

                                    Dostępne akcje:
                                    REPAIR - jeżli zdjęcie jest uszkodzone
                                    DARKEN - jeżli zdjęcie jest zbyt jasne
                                    BRIGHTEN - jeżli zdjęcie jest zbyt ciemne

                                    <PRZYKŁAD>
                                    REPAIR
                                    <PRZYKŁAD>
                                    """;

        var action = await VisionImage.Create(visionPrompt, imageUrl)
            .Bind(async image => await visionClient.Recognize(image))
            .GetValueOrDefault(x => x.Text);
        
        // STEP 3. Napraw uszkodzone zdjęcia
        var command = $"{action} {imageName}";
        
        var repairedPhoto = await ReportResult("photos", taskResult: command);
        
        // STEP 4. Zwróć opis naprawionych zdjęć
        var fixedImageUrl = "https://centrala.ag3nts.org/dane/barbara/IMG_1443_FT12.PNG";
        
        var description = await VisionImage.Create("Zwróć szczegółówy opis zdjęcia PO POLSKU, nie pomiń żadnego szczegółu.", fixedImageUrl)
            .Bind(async image => await visionClient.Recognize(image))
            .GetValueOrDefault(x => x.Text);
        
        return await ReportResult("photos", taskResult: description);
    }

    public async Task<Result<string>> Task_02()
    {
        const string systemPrompt = "Ocenisz czy podany tekst czy nie na podstawie otrzymanego wzorca";

        // await PrepareFineTuningFile();

        const string verifyTxt = "Data/Lab/verify.txt";

        var verifyText = await File.ReadAllTextAsync(verifyTxt);

        var verifyList = verifyText.Split("\n").ToList();

        var taskResult = new List<string>();

        foreach (var record in verifyList.Where(v => v != string.Empty))
        {
            var line = record.Split("=");

            var fileName = line[0];
            var text = line[1];

            var result = await Prompt.Create(text)
                .Bind(prompt => prompt.ChooseModel("ft:gpt-4o-mini-2024-07-18:personal:aidevs3:Aj47x9N3"))
                .Bind(prompt => prompt.ConfigureSystem(systemPrompt))
                .Bind(async prompt => await llmClient.GenerateAnswer(prompt))
                .GetValueOrDefault(answer => answer.Text);
            
            if (result == "YES")
                taskResult.Add(fileName);
        }

        return await ReportResult("research", taskResult: taskResult);

        async Task PrepareFineTuningFile()
        {
            const string incorrectTxt = "Data/Lab/incorrect.txt";
            const string correctTxt = "Data/Lab/correct.txt";

            var incorrectText = await File.ReadAllTextAsync(incorrectTxt);
            var correctText = await File.ReadAllTextAsync(correctTxt);

            var incorrectList = incorrectText.Split("\n").ToList();
            var correctList = correctText.Split("\n").ToList();
            
            var jsonLText = ProcessFineTuningModels(incorrectList, correctList);

            await File.WriteAllTextAsync("Data/Lab/fineTuning.jsonl", jsonLText);
        }
        
        string ProcessFineTuningModels(List<string> incorrectList, List<string> correctList)
        {
            var fineTuningModels = new List<FineTuningModel>();

            fineTuningModels.AddRange(GenerateFineTuningModels(incorrectList, systemPrompt, "NO"));
            fineTuningModels.AddRange(GenerateFineTuningModels(correctList, systemPrompt, "YES"));

            var text = new StringBuilder();
            
            foreach (var line in fineTuningModels)
            {
                text.AppendLine(JsonSerializer.Serialize(line));
            }

            return text.ToString();
        }

        static List<FineTuningModel> GenerateFineTuningModels(
            IEnumerable<string> records,
            string systemPrompt,
            string assistantResponse)
        {
            var fineTuningModels = new List<FineTuningModel>();

            foreach (var record in records)
            {
                var fineTuningMessage = new List<FineTuningMessage>
                {
                    new()
                    {
                        Role = "system",
                        Content = systemPrompt
                    },
                    new()
                    {
                        Role = "user",
                        Content = record
                    },
                    new()
                    {
                        Role = "assistant",
                        Content = assistantResponse
                    }
                };

                fineTuningModels.Add(new FineTuningModel
                {
                    Messages = fineTuningMessage
                });
            }

            return fineTuningModels;
        }
    }

    public async Task<Result<string>> Task_03()
    {
        const string url = $"https://centrala.ag3nts.org/data/{ApiKey}/softo.json";

        var softoData = await _httpClient.GetStringAsync(url);

        var questions = JsonSerializer.Deserialize<Dictionary<string, string>>(softoData);

        const string crawlUrl = "https://softo.ag3nts.org";

        // var initCrawl = await firecrawlService.InitCrawlAsync(crawlUrl);

        var pagesDetails = await firecrawlService.CrawlResultAsync("cf7e3926-722f-48c8-99b0-e30763332f88")
            .GetValueOrDefault(x => x);

        var sources = new StringBuilder();

        foreach (var page in pagesDetails!)
        {
            sources.AppendLine($"<ŹRÓDŁO>{page.Url}</ŹRÓDŁO>");
        }

        var result = new Dictionary<string, string>();
        
        foreach (var question in questions!)
        {
            var masterPrompt = $"""
                                Twoim jest wybranie źródeł informacji, które pomogą odpowiedzieć na pytanie:
                                <PYTANIE>
                                {question.Value}
                                </PYTANIE>

                                Żródła informacji:
                                {sources}

                                <ZASADY>
                                1. Zwróć tylko i wyłącznie adresy URL, według przykładu
                                2. Możesz zwrócić więcej niż 1 adres, jeżeli uważasz, że dana strona może zawierać niezbędne informacje
                                <ZASADY>

                                <PRZYKŁAD>
                                https://softo.ag3nts.org
                                <PRZYKŁAD>

                                <PRZYKŁAD>
                                https://softo.ag3nts.org/porfolio, https://softo.ag3nts.org/about, https://softo.ag3nts.org/contact
                                <PRZYKŁAD>
                                """;

            var sourcesToVerify = await Prompt.Create(masterPrompt)
                .Bind(async prompt => await llmClient.GenerateAnswer(prompt))
                .GetValueOrDefault(x => x.Text);

            var urlsToCheck = sourcesToVerify!.Split(",").Select(x => x.Trim());

            var context = new StringBuilder();

            pagesDetails.ForEach(page =>
            {
                if (!urlsToCheck.Contains(page.Url)) return;

                context.AppendLine($"<ŹRÓDŁO>{page.Url}</ŹRÓDŁO>");
                context.AppendLine($"<TYTUŁ>{page.Title}</TYTUŁ>");
                context.AppendLine($"<ZAWARTOŚĆ>{page.Content}</ZAWARTOŚĆ>");
                context.AppendLine($"<LINKI>{page.Links}</LINKI>");
            });

            var answerPrompt = $"""
                                Odpowiedz na pytanie na podstawie zebranych informacji i zwróć odpowiedź w formie krótkiej i zwięzłej.
                                <PYTANIE>
                                {question.Value}
                                </PYTANIE>

                                <ZASADY>
                                1. Zwróć tylko i wyłącznie odpowiedź na pytanie, bez dodatkowych informacji i zdań
                                2. Bądź bardzo konkretny i zwięzły
                                </ZASADY>
                                
                                <KONTEKST>
                                {context}
                                </KONTEKST>
                                """;
            
            var answer = await Prompt.Create(answerPrompt)
                .Bind(async prompt => await llmClient.GenerateAnswer(prompt))
                .GetValueOrDefault(x => x.Text);
            

            result.Add(question.Key, answer!);
        }

        return await ReportResult("softo", taskResult: result);
    }

    public async Task<Result<string>> Task_04()
    {
        const string apiUrl = "https://azyl-52752.ag3nts.org/api/instructions";

        return await ReportResult("webhook", taskResult: apiUrl);
    }

    public async Task<Result<string>> Task_05()
    {
        var questions = await GetQuestions();

        var extractedData = await pdf4MeService.ExtractDataAsync("Data/notatnik.pdf")
            .GetValueOrDefault(x => x);

        if (extractedData == null || extractedData.Pages.Count == 0)
            return Result.Failure<string>("Failed to extract data from the document.");

        var textContext = await BuildContext();
        
        var facts = await GetFacts();

        var additionalInfo = await GetAdditionalInformation();
        
        var answers = await GetAnswers();

        return await ReportResult("notes", taskResult: answers);

        async Task<Dictionary<string, string>?> GetQuestions()
        {
            const string url = $"https://centrala.ag3nts.org/data/{ApiKey}/notes.json";

            var result = await _httpClient.GetStringAsync(url);

            return JsonSerializer.Deserialize<Dictionary<string, string>>(result);
        }

        async Task<Dictionary<string, string>> GetAdditionalInformation()
        {
            var additionalInformation = new Dictionary<string, string>();

            var forbiddenAnswers = new Dictionary<string, List<string>?>
            {
                { "01", ["2024", "2022", "2023", "2021"] },
                { "02", ["Azazel"] },
                { "03", ["Teren poza miastem.", "Lubawa koło Grudziądza.", "Miejsce poza miastem."] }
            };
        
            foreach (var question in questions!)
            {
                forbiddenAnswers.TryGetValue(question.Key, out var forbidden);

                var forbiddenText = forbidden != null ? string.Join(", ", forbidden) : "Brak";

                var masterPrompt = $"""
                                   Przeprowadz analizę tekstu i odpowiedz na pytanie, zwracając uwagę na zakazane wartości.
                                   W tekście znajdują się informacje, które pomogą Ci znaleźć odpowiedź na pytanie.
                                   Przeprowadz cały tok rozumowania 

                                   <ZASADY>
                                   1. WYKLUCZ Z ROZUMOWANIA ZAKAZANE WARTOŚCI,
                                   2. ZACZNIJ OD WNIOSKÓW, A NASTĘPNIE PRZEJDŹ DO ODPOWIEDZI NA PYTANIE
                                   </ZASADY>

                                   <PYTANIE>
                                   {question.Value}
                                   </PYTANIE>

                                   <KONTEKST>
                                   {textContext}
                                   </KONTEKST>

                                   <ZAKAZANE WARTOŚCI>
                                   {forbiddenText}
                                   </ZAKAZANE WARTOŚCI>

                                   ZWRÓĆ ODPOWIEDŹ W FORMACIE:

                                   ** OTO MOJE ROZUMOWANIE **
                                   tutaj wpisz swoje rozumowanie

                                   ** WNIOSKI **
                                   tutaj wpisz swoje wnioski

                                   ** ODPOWIEDŹ NA PYTANIE **
                                   twoja odpowiedź
                                   """;

                var answer = await Prompt.Create(masterPrompt)
                    .Bind(async prompt => await llmClient.GenerateAnswer(prompt))
                    .GetValueOrDefault(x => x.Text);

                additionalInformation.Add(question.Key, answer!);
            }

            return additionalInformation;
        }

        async Task<Dictionary<string, string>> GetAnswers()
        {
            var answerResult = new Dictionary<string, string>();
            
            foreach (var question in questions!)
            { 
                var additionalInfoText = additionalInfo[question.Key];

                var masterPrompt = $"""
                                   Odpowiedz na pytanie na podstawie zebranych informacji i 
                                   zwróć odpowiedź w formie krótkiej i zwięzłej.

                                   <ZASADY>
                                   1. Zwróć tylko i wyłącznie odpowiedź na pytanie, bez dodatkowych informacji i zdań
                                   2. Bądź bardzo konkretny i zwięzły
                                   </ZASADY>

                                   <PYTANIE>
                                   {question.Value}
                                   </PYTANIE>

                                   <KONTEKST>
                                   {textContext}
                                   </KONTEKST>
                                    
                                   <DODATKOWE ROZUMOWANIE>
                                   {additionalInfoText}
                                   </DODATKOWE ROZUMOWANIE>
                                   """;

                var answer = await Prompt.Create(masterPrompt)
                    .Bind(async prompt => await llmClient.GenerateAnswer(prompt))
                    .GetValueOrDefault(x => x.Text);

                answerResult.Add(question.Key, answer!);
            }

            return answerResult;
        }

        async Task<string> BuildContext()
        {
            var context = new StringBuilder();
            
            foreach (var page in extractedData.Pages)
            {
                context.AppendLine("<STRONA>");
                context.AppendLine("<TEKST>");
            
                var correctionPrompt = $"""
                                        Zredaguj tekst tak aby był czytelny i zrozumiały. Uzupełnij informacje, które pomogą 
                                        lepiej zrozumieć przedstawione fakty i wydarzenia. 

                                        <ZASADY>
                                        1. Kluczowe są wszystkie informacje takie jak daty, miejsca, nazwy, które pomogą zrozumieć kontekst
                                        </ZASADY>

                                        <DO REDAKCJI>
                                        {page.Text}
                                        </DO REDAKCJI>
                                        """;
            
                var editedText = await Prompt.Create(correctionPrompt)
                    .Bind(async prompt => await llmClient.GenerateAnswer(prompt))
                    .GetValueOrDefault(x => x.Text);
            
                context.AppendLine(editedText);
            
                context.AppendLine("</TEKST>");
            
                foreach(var image in page.ImagesBase64)
                {
                    context.AppendLine("<ZDJĘCIE>");
                
                    var visionPrompt = $"""
                                        Rozpoznaj tekst lub obiekt na zdjęciu i zwróć jego opis/treść PO POLSKU. 

                                        <ZASADY>
                                        1. Treść ma być PO POLSKU
                                        </ZASADY>

                                        Zdjęcie znajduje się na stronie pliku PDF na którym znajduje się tekst:

                                        <TEKST>
                                        {editedText}
                                        </TEKST>
                                        """;
                
                    var imageDescription = await VisionImage.Create(visionPrompt, image, "png")
                        .Bind(async visionImage => await visionClient.Recognize(visionImage))
                        .GetValueOrDefault(x => x.Text);
                
                    context.AppendLine(imageDescription);
                
                    context.AppendLine("</ZDJĘCIE>");
                }
        
                context.AppendLine("</STRONA>");
            }

            return context.ToString();
        }

        async Task<Dictionary<string, string>> GetFacts()
        {
            var factsResult = new Dictionary<string, string>();
            
            var factsSystemPrompt = "Jesteś śledczym który analizuje wszystkie fakty, " +
                                    "wydarzenia miejsca i daty, uzupełnia brakujące informacj";

            var factsPrompt = """
                              Na podstawie otrzymanego tekstu zwróć listę faktów z datami, miejscami osobami w formie listy

                              <ZASADY>
                              1. Kluczowe są wszystkie informacje takie jak daty, miejsca, nazwy, które pomogą zrozumieć kontekst
                              </ZASADY>

                              <KONTEKST>
                              {editedText}
                              </KONTEKST>

                              <PRZYKŁAD>
                              1. [FAKT] - [DATA] - [MIEJSCE] - [OSOBA] 
                              2. [FAKT] - [DATA] - [MIEJSCE] - [OSOBA] 
                              ....
                              </PRZYKŁAD>
                              """;
            
            return factsResult;
        }
    }

    private static async Task<Result<string>> RunPythonScript(string dataPath, string scriptName, string argument)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "python3",
                Arguments = $"{dataPath}/{scriptName} {dataPath}/{argument}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardOutputEncoding = Encoding.UTF8,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        process.Start();

        var output = await process.StandardOutput.ReadToEndAsync();
        var error = await process.StandardError.ReadToEndAsync();

        await process.WaitForExitAsync();

        return process.ExitCode != 0
            ? Result.Failure<string>($"Python script error: {error}")
            : output.Trim();
    }
}