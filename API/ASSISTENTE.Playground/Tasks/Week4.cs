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
    public async Task<Result<string>> Task_01()
    {
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
}