using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using ASSISTENTE.Infrastructure.Audio.Contracts;
using ASSISTENTE.Infrastructure.Embeddings.Contracts;
using ASSISTENTE.Infrastructure.Firecrawl.Contracts;
using ASSISTENTE.Infrastructure.Image.Contracts;
using ASSISTENTE.Infrastructure.LLM.Contracts;
using ASSISTENTE.Infrastructure.MarkDownParser.Contracts;
using ASSISTENTE.Infrastructure.Neo4J.Contracts;
using ASSISTENTE.Infrastructure.Qdrant.Contracts;
using ASSISTENTE.Infrastructure.Vision.Contracts;
using ASSISTENTE.Playground.Models;
using ASSISTENTE.Playground.Models.DataApiModels.DataModels;
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
    IQdrantService qdrantService) : TaskBase(httpClient)
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
                    new FineTuningMessage
                    {
                        Role = "system",
                        Content = systemPrompt
                    },
                    new FineTuningMessage
                    {
                        Role = "user",
                        Content = record
                    },
                    new FineTuningMessage
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
}