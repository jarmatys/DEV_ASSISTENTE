using System.Text;
using ASSISTENTE.Infrastructure.Audio.Contracts;
using ASSISTENTE.Infrastructure.Embeddings.Contracts;
using ASSISTENTE.Infrastructure.Firecrawl.Contracts;
using ASSISTENTE.Infrastructure.Image.Contracts;
using ASSISTENTE.Infrastructure.LLM.Contracts;
using ASSISTENTE.Infrastructure.MarkDownParser.Contracts;
using ASSISTENTE.Infrastructure.Qdrant.Contracts;
using ASSISTENTE.Infrastructure.Vision.Contracts;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Playground.Tasks;

public class WeekThree(
    HttpClient httpClient,
    ILlmClient llmClient,
    IAudioClient audioClient,
    IVisionClient visionClient,
    IImageClient imageClient,
    IFirecrawlService firecrawlService,
    IMarkDownParser markDownParser,
    IEmbeddingClient embeddingClient,
    IQdrantService qdrantService) : TaskBase(httpClient)
{
    public async Task<Result<string>> Task_01()
    {
        const string reportFiles = "Data/Files";
        const string factFiles = "Data/Files/Facts";

        var factContext = await GetFactContext();

        var keyWords = new Dictionary<string, string>();

        var reportFilePaths = Directory.GetFiles(reportFiles);

        foreach (var reportFile in reportFilePaths)
        {
            var fileName = Path.GetFileName(reportFile);
            var extension = Path.GetExtension(reportFile);

            if (extension != ".txt")
                continue;

            var currentReport = await File.ReadAllTextAsync(reportFile);

            var promptTextStep1 =
                $"""
                 Twoje zadanie to sprawdzić czy w faktach (<FAKTY> znajduje się informacje pasujące tematycznie do aktualnie 
                 odczytywanego raportu <RAPORT> (np. łączy je osoba lub miejsce).
                 Jeżeli znajdziesz takie powiązanie zwróć nazwę pliku z faktem, który pasuje do raportu.

                 <FAKTY>
                 {factContext}
                 </FAKTY>

                 <RAPORT>
                 {currentReport}
                 </RAPORT>

                 Zwróć tylko i wyłącznie nazwę pliku z faktem, który pasuje do raportu np. f01.txt. (bez cudzysłowów i znaków specjalnych)

                 Lista dostępnych plików z faktami: {string.Join(", ", Directory.GetFiles(factFiles))}

                 Jeżeli nie znajdziesz pasującego faktu, zwróć "BRAK".
                 """;

            var factFileName = await Prompt.Create(promptTextStep1)
                .Bind(async prompt => await llmClient.GenerateAnswer(prompt))
                .GetValueOrDefault(x => x.Text, "");

            var masterContext = new StringBuilder();

            if (factFileName != "BRAK")
            {
                var factFile = await File.ReadAllTextAsync($"Data/Files/Facts/{factFileName}");

                masterContext.Append($"<SEKTOR>{fileName}</SEKTOR>\n" +
                                     $"<RAPORT>{currentReport}</RAPORT>\n" +
                                     $"<FAKT>{factFile}</FAKT>\n\n");
            }
            else
            {
                masterContext.Append($"<SEKTOR>{fileName}</SEKTOR>\n" +
                                     $"<RAPORT>{currentReport}</RAPORT>\n");
            }

            var masterPrompt =
                $"""
                 Twoim zadaniem jest wygenerowanie listy słów kluczowych dla otrzymanego tekstu. Postępuj zgodnie z poniższymi instrukcjami:

                 <INSTRUKCJA>
                 1. Wygeneruj listę słów kluczowych w języku polskim dla otrzymanego tekstu. 
                 2. Podaj wszystkie możliwe słowa kluczowe.
                 3. Słowa kluczowe muszą być w formie mianownika (np. "fabryka", nie "fabryką").
                 4. Format: słowa kluczowe oddzielone przecinkami, bez dodatkowych znaków (np. "sektor, fabryka, incydent").
                 5. Uwzględnij w tagach nazwy sektorów z nazw plików np. C4, A1 
                 </INSTRUKCJA>

                 <ZASADY>
                 1. Zwróć wyłącznie listę słów kluczowych. Nie dodawaj żadnych dodatkowych informacji ani opisów.
                 2. Słowa kluczowe muszą być zgodne z powyższymi wytycznymi.
                 </ZASADY>

                 <PRZYKŁAD 1>
                 Fragment tekstu: "Aresztowanie Aleksandra Ragowskiego za działania przeciwko reżimowi..."
                 Słowa kluczowe: "aresztowanie, Aleksander Ragowski, działania, reżim"
                 </PRZYKŁAD 1>

                 <PRZYKŁAD 2>
                 Fragment tekstu: "Zawód: Nauczyciel języka angielskiego/formerly związany z edukacją...."
                 Słowa kluczowe: "nauczyciel, język angielski, edukacja"
                 </PRZYKŁAD 2>

                 <TEXT>
                 {masterContext}
                 </TEXT>

                 Na podstawie powyższych instrukcji, wygeneruj listę słów kluczowych dla otrzymanego tekstu.
                 """;

            await Prompt.Create(masterPrompt)
                .Bind(async prompt => await llmClient.GenerateAnswer(prompt))
                .Tap(keyWord => keyWords.Add(fileName, keyWord.Text));
        }

        return await ReportResult("dokumenty", keyWords);

        async Task<string> GetFactContext()
        {
            var factFilePaths = Directory.GetFiles(factFiles);

            var fullFactContext = new StringBuilder();
            foreach (var factFile in factFilePaths)
            {
                var fileName = Path.GetFileName(factFile);

                var fileContent = await File.ReadAllTextAsync(factFile);

                fullFactContext.Append($"<PLIK>{fileName}</PLIK>\n<FAKT>{fileContent}</FAKT>\n\n");
            }

            return fullFactContext.ToString();
        }
    }

    public async Task<Result<string>> Task_02()
    {
        const string dataFilesWeapons = "Data/Files/Weapons";
        const string question = "W raporcie, z którego dnia znajduje się wzmianka o kradzieży prototypu broni?";

        var weaponsFiles = Directory.GetFiles(dataFilesWeapons);
        
        await qdrantService.DropCollectionAsync("aidevs");
        await qdrantService.CreateCollectionAsync("aidevs");
        
        foreach (var weaponFile in weaponsFiles)
        {
            var fileName = Path.GetFileName(weaponFile);
            var fileContent = await File.ReadAllTextAsync(weaponFile);
        
            await EmbeddingText.Create(fileContent)
                .Bind(async embeddingText => await embeddingClient.GetAsync(embeddingText))
                .Bind(embedding =>
                {
                    var metadata = new Dictionary<string, string>
                    {
                        { "fileName", fileName }
                    };
        
                    return DocumentDto.Create("aidevs", embedding.Embeddings, metadata);
                })
                .Bind(async document => await qdrantService.UpsertAsync(document));
        }

        var date = await EmbeddingText.Create(question)
            .Bind(async embeddingText => await embeddingClient.GetAsync(embeddingText))
            .Bind(embedding => VectorDto.Create("aidevs", embedding.Embeddings, elements: 1))
            .Bind(async document => await qdrantService.SearchAsync(document))
            .Map(documents => documents.First())
            .Map(document => document.Metadata.GetValueOrDefault("fileName"))
            .Map(Path.GetFileNameWithoutExtension)
            .Map(dateString => DateTime.Parse(dateString!.Replace("_", "-")).ToString("yyyy-MM-dd"))
            .GetValueOrDefault(x => x);
        

        return await ReportResult("wektory", date);
    }
}