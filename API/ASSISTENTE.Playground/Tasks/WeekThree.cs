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

        // TODO: Create library 'ASSISTENTE.Infrastructure.Search' which will be combined qdrant, embedding and other services like typsense, database simple query
        // Implement hybrid search service which will be able to search in multiple sources and score them together
        // Combined score formula: (1 / vector rank) + (1 / full text rank)

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

    public async Task<Result<string>> Task_03()
    {
        // Dostępne tabele: users, datacenters & connections

        const string question = "które aktywne datacenter (DC_ID) są zarządzane przez pracowników, " +
                                "którzy są na urlopie (is_active=0)";

        const string acceptableFormat = "1, 2, 3";

        var context = new StringBuilder();

        while (true)
        {
            var masterContext = context.ToString() == string.Empty
                ? "BRAK INFORMACJI - NAPISZ ZAPYTANIE SQL"
                : context.ToString();

            if (context.ToString() != string.Empty)
            {
                var verifyPrompt = $"""
                                    Twoim zadaniem jest zweryfikować czy w pozyskanych informacjach znajduje się 
                                    odpowiedź na zadane pytanie.

                                    <PYTANIE>
                                    {question}
                                    </PYTANIE>

                                    <KONTEKST>
                                    {masterContext}
                                     </KONTEKST>


                                    Jeżeli nie znajdziesz odpowiedzi, zwróć "BRAK".

                                    Jeżeli znajdziesz odpowiedź, zwróć znalezioną informację.

                                    Zwrócona informacja powinna zawierać tylko i wyłącznie odpowiedź na pytanie 
                                    w formacie: {acceptableFormat}
                                    """;

                var hasAnswer = await Prompt.Create(verifyPrompt)
                    .Bind(async prompt => await llmClient.GenerateAnswer(prompt))
                    .GetValueOrDefault(x => x.Text, "");

                if (hasAnswer != "BRAK")
                {
                    var inActiveDataCenterIds = hasAnswer.Split(", ")
                        .Select(int.Parse)
                        .ToList();

                    return await ReportResult("database", inActiveDataCenterIds);
                }
            }

            var masterPrompt = $"""
                                Twoim zadaniem jest otrzymać odpowiedź na pytanie:
                                <PYTANIE>
                                {question}
                                </PYTANIE>

                                <ZASADY>
                                1. Do dyspozycji masz dostęp do bazy danych. Musisz napisać zapytania SQL, które naprowadzą Cię na odpowiedź.
                                2. Nie podawaj żadnych dodatkowych informacji, zwróć sam SQL lub odpowiedź na pytanie.
                                3. Nie zwracaj żadnych tagów markdown, ani znaków specjalnych, same zapytanie SQL.
                                </ZASADY>

                                <PRZYDATNE ZAPYTANIA>
                                `show tables` = zwraca listę tabel
                                `show create table <NAZWA_TABELI>` = pokazuje, jak zbudowana jest konkretna tabela
                                </PRZYDATNE ZAPYTANIA>

                                Jeżeli nie jesteś pewny jak odpowiedzieć na zadane pytanie przygotuj kolejne zapytanie SQL aby uzyskać więcej szczegółów.
                                Jeżeli masz wszystko czego potrzebujesz zwróć rezultat w postaci liczb po przecinku np. 1, 2, 3.

                                <PRZYKŁAD ODPOWIEDZI>
                                select * from datacenters
                                </PRZYKŁAD ODPOWIEDZI>

                                <POZYSKANE INFORMACJE>
                                {masterContext}
                                </POZYSKANE INFORMACJE>
                                """;

            var sqlQuery = await Prompt.Create(masterPrompt)
                .Bind(async prompt => await llmClient.GenerateAnswer(prompt))
                .GetValueOrDefault(x => x.Text, "");

            var isSqlQuery = sqlQuery.Contains("select", StringComparison.OrdinalIgnoreCase) ||
                             sqlQuery.Contains("show", StringComparison.OrdinalIgnoreCase);

            if (isSqlQuery)
            {
                var queryResult = await DatabaseQuery("database", sqlQuery);

                context.Append($"<WYKONANE ZAPYTANIE>{sqlQuery}</WYKONANE ZAPYTANIE>\n" +
                               $"<REZULTAT>{queryResult}</REZULTAT>\n\n");

                continue;
            }

            break;
        }

        return Result.Success("Zadanie zakończone");
    }

    public async Task<Result<string>> Task_04()
    {
        const string question = "Twoim zadaniem jest zweryfikowanie wszystkich kombinacji imion i miast na podstawie " +
                                "dostępnych informacji. Sprawdzaj osoby, dostaniesz informacje o miastach, a nastepnie " +
                                "sprawdzaj miasta, dostaniesz informacje o osobach.";

        const string placesDescription = """
                                         Pierwszy z nich to wyszukiwarka członków ruchu oporu. Możemy wyszukiwać ich z 
                                         użyciem imienia podanego w formie mianownika, a w odpowiedzi otrzymamy listę 
                                         miejsc, w których ich widziano.

                                         <PRZYKŁAD>
                                         RAFAL
                                         <PRZYKŁAD>
                                         """;

        const string peopleDescription = """
                                         Drugi system to wyszukiwarka miejsc odwiedzonych przez konkretne osoby.
                                         Podajesz nazwę miasta do sprawdzenia (bez polskich znaków) i w odpowiedzi 
                                         dowiadujesz się, których z członków ruchu oporu tam widziano.

                                         <PRZYKŁAD>
                                         WARSZAWA
                                         <PRZYKŁAD>
                                         """;

        var introInformation = await File.ReadAllTextAsync("Data/barbara.txt");

        const string restrictedData = "[**RESTRICTED DATA**]";

        var placesWithPeople = new Dictionary<string, List<string>>();
        var peopleWithPlaces = new Dictionary<string, List<string>>();

        var checkedPlaces = new List<string>();
        var checkedPeople = new List<string>();

        var forbiddenRecords = new List<string>();

        return Result.Success("");

        string MasterPrompt()
        {
            return """
                   Twoim zadaniem jest znalezienie odpowiedzi na pytanie:
                   <PYTANIE>
                   {question}
                   </PYTANIE>

                   <KONTEKST>
                   {introInformation}
                   </KONTEKST>

                   <ZASADY>
                   1. Jeżeli w zebranych informacjach nie udało się namierzyć odpowiedzi, wykonaj kolejne zapytanie!
                   2. Zwracaj tylko i wyłącznie nazwę miasta lub osoby duzymi literami bez polskich znaków!
                   3. Nie formatuj tekstu i nie zwracaj markdown oraz znaków specjalnych!
                   4. Nie używaj polskich znaków!
                   5. NIE UŻYWAJ ZAKAZANYCH WARTOŚCI!
                   </ZASADY>

                   <ZAKAZANE WARTOŚCI>
                   {forbiddenRecords}
                   </ZAKAZANE WARTOŚCI>

                   <DOSTĘPNE SYSTEMY - WERYFIKACJA MIEJSC>
                   {placesDescription}
                   </DOSTĘPNE SYSTEMY - WERYFIKACJA MIEJSC>

                   <DOSTĘPNE SYSTEMY - WERYFIKACJA OSÓB>
                   {peopleDescription}
                   </DOSTĘPNE SYSTEMY - WERYFIKACJA OSÓB>

                   <ZEBRANE INFORMACJE>
                   OSOBY DO SPRAWDZENIA: {string.Join(", ", peopleToCheck)}
                   MIEJSCA DO SPRAWDZENIA: {string.Join(", ", placesToCheck)}

                   SPRAWDZIŁEŚ JUŻ:
                   SPRAWDZONE OSOBY: {string.Join(", ", checkedPeople)}
                   SPRAWDZONE MIEJSCA: {string.Join(", ", checkedPlaces)}
                   </ZEBRANE INFORMACJE>

                   Na podstawie zebranych informacji oraz kontekstu, generuj zapytania do people api i 
                   places api według podanego formatu. Zwróć tylko i wyłącznie to czego wymaga dane zapytanie.
                   """;
        }

        string VerifyPrompt(string answer)
        {
            return $"""
                    Rozpoznaj czy otrzymana wartość dotyczy osób czy miejsc

                    <WARTOŚĆ>
                    {answer}
                    </WARTOŚĆ>

                    Otrzymana wartość nie może zawierać polskich znaków, jeżeli wykryjesz polskie znaki 
                    zwróć error.

                    <PRZYKŁAD>
                    Otrzymałeś: BARBARA
                    Zwróć: PEOPLE
                    </PRZYKŁAD>

                    <PRZYKŁAD>
                    Otrzymałeś: WARSZAWA
                    Zwróć: PLACES
                    </PRZYKŁAD>

                    <PRZYKŁAD>
                    Otrzymałeś: Nie mamy wystarczajacych informacji w zebranych danych, aby okreslic aktualne miejsce pobytu Barbary. Spróbuje uzyskac wiecej informacji.
                    Zwróć: ERROR
                    </PRZYKŁAD>

                    <PRZYKŁAD>
                    Otrzymałeś: RAFAŁ
                    Zwróć: ERROR
                    </PRZYKŁAD>

                    NIE ZWRACAJ NIC POZA WARTOŚCIĄ: PLACES, PEOPLE, ERROR
                    """;
        }
    }
}