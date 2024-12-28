using ASSISTENTE.Infrastructure.Pdf4Me.Contracts;
using CSharpFunctionalExtensions;
using Pdf4meClient;

namespace ASSISTENTE.Infrastructure.Pdf4Me;

internal sealed class Pdf4MeService(Pdf4me client) : IPdf4MeService
{
    public async Task<Result<ExtractData>> ExtractDataAsync(string filePath)
    {
        var fullPath = Path.GetFullPath(filePath);
        var fileName = Path.GetFileName(fullPath);

        var request = new ExtractResources
        {
            Document = new Document
            {
                DocData = await File.ReadAllBytesAsync(fullPath),
                Name = fileName,
            },
            ExtractResourcesAction = new ExtractResourcesAction
            {
                ExtractFonts = true,
                ExtractImages = true,
                Outlines = true,
                XmpMetadata = true,
                ListFonts = true,
                ListImages = true,
                ExtractText = true,
                SaveText = true
            }
        };

        var resources = await client.ExtractClient.ExtractResourcesAsync(request);

        if (resources == null)
            return Result.Failure<ExtractData>("Failed to extract resources from the document.");

        var pageCount = (int)resources.PdfResources.DocMetadata.PageCount;

        var images = resources.PdfResources.Images
            .Select(x => new { x.Order, x.DocData })
            .DistinctBy(x => x.DocData.Length)
            .ToList();

        var textChunks = resources.PdfResources.Text
            .Select(x => new { x.Order, x.DocText })
            .ToList();

        var pagesContent = new List<PageContent>();

        foreach (var page in Enumerable.Range(1, pageCount))
        {
            var pageText = textChunks.Single(x => x.Order == page);

            var pageImages = images
                .Where(x => x.Order == page)
                .Select(x => x.DocData)
                .ToList();

            var result = PageContent.Create(pageText.DocText, pageImages)
                .Tap(pageContent => pagesContent.Add(pageContent));

            if (result.IsFailure)
                return Result.Failure<ExtractData>(result.Error);
        }

        var fullText = string.Join(Environment.NewLine, textChunks.Select(x => x.DocText));

        return ExtractData.Create(fullText, pagesContent);
    }
}